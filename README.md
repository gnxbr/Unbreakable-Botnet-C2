
## Unbreakable Botnet C2

### Intro

Recently I have read a [news](https://arstechnica.com/information-technology/2021/02/crooks-use-the-bitcoin-blockchain-to-protect-their-botnets-from-takedown/) about a new technique caught in the wild, that was being used by a Threat Actor.
It was using the blockchain of Bitcoin to spread an IP of C2 for the bots from a botnet.
I have decided creating my own version of it, using the blockchain of Litecoin and an IRC Bot written in C# as a Proof of Concept.

The idea can evolve and be useful in a C2 Infrasctructure for Red Team Engagements.

If used by a Malicious Agent, Law Enforcement can't take it down, because of the blockchain nature (descentralized network).

### How it works

The IRC bot uses an API of a Block Explorer site and queries the values of the last 3 transations to verify the IP for the C2 Server (ircd in this case).
The most recent transaction value needs to be 31337 (0.00031337 LTC), it is like an "initialization flag". 
The next 2 values are the IP, divided in 2 blocks of 5 chars (can be less, depends on IP), in its integer form.
Currently I'm not obfuscating the IP in any way.

It takes less than 5 minutes to change the IP (transactions to be confirmed).
I set it up to spend less than R$ 2,00 (0.34 dollar cents) at each change.
During my experiments, I spent R$ 1,74 (0.31 dollar cents), when the LTC price was R$ 1.115,64 (US$ 197,58).

It's very cheap to change the IP. The money can be transferred between the Threat Actor accounts, therefore very little is spent in fees. 
The negative side of it is that anyone that figure the logic out can change the IP.
One possible solution for this problem, the most brilliant idea I had, is changing the expected value (the part that does not identify the IP itself) to a fixed and high amount.
Well, the IP would be changed and the money would be on the attacker's account, hehe. 
And, of course, for the change take effect, the infected boxes or the ircd machine must be restarted (only the attacker can do it).

### Conclusion

The idea is very cool!
Thinking about the negative effect: it will bring problems for the Law Enforcement to takeover the botnets using this technique.
Thinking about the positive effect: the idea can evolve and be useful for Red Teams, for example!

### Source

Have fun: [src folder](src/)

The IP used was *3105793573* (185.30.166.37) from Freenode.

### Demo

Watch on [YouTube](https://www.youtube.com/watch?v=L-tx1Act7ao)

### Greetz

[Saullo "n0ps13d" Carvalho](https://twitter.com/n0ps13d)

### Contact

If you wanna talk more about it, get in touch:

* **IRC:** gnx @ freenode
* **Telegram:** [@gnxbr](https://t.me/gnxbr)
* **Twitter:** [@alissonbertochi](https://twitter.com/alissonbertochi)
* **E-mail:** alisson[at]bertochi[.]com[.]br
* **Linkedin:** [https://www.linkedin.com/in/gnxbr/](https://www.linkedin.com/in/gnxbr/)
