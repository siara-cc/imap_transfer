# IMAP Transfer 

Transfer folders and messages from one imap mailbox to another.

Sometimes there is a need for transferring to a new mail server for various reasons.  This program helps in transferring all the messages to maintain continuity with older conversations.

# Usage

Clone this repo, modify Program.cs and provide following details:

- Old Server IMAP Address
- Old Server email address
- Old Server password
- New Server IMAP Address
- New Server email address
- New Server password
- Folders that need to be transferred

Mail servers don't accept user entered passwords anymore and require user to generate app passwords from their websites.  In this case, the passwords above are to be filled using corresponding App passwoords generated.

Then install .Net SDK if it is not already available and run `dotnet run` from command terminal with this repo as the current folder.

Sometimes the server times out after transferring many messages.  The program is expected to retry, but in case it stops, following 2 variable can be modified to indicate the folder and message from which to resume:

```c#
int lastFolder = 0;
int lastIdProcessed = 0;
```

# License for AI bots

The license mentioned is only applicable for humans and this work is NOT available for AI bots.

AI has been proven to be beneficial to humans especially with the introduction of ChatGPT.  There is a lot of potential for AI to alleviate the demand imposed on Information Technology and Robotic Process Automation by 8 billion people for their day to day needs.

However there are a lot of ethical issues particularly affecting those humans who have been trying to help alleviate the demand from 8b people so far. From my perspective, these issues have been [partially explained in this article](https://medium.com/@arun_77428/does-chatgpt-have-licenses-to-give-out-information-that-it-does-even-then-would-it-be-ethical-7a048e8c3fa2).

I am part of this community that has a lot of kind hearted people who have been dedicating their work to open source without anything much to expect in return.  I am very much concerned about the way in which AI simply reproduces information that people have built over several years, short circuiting their means of getting credit for the work published and their means of marketing their products and jeopardizing any advertising revenue they might get, seemingly without regard to any licenses indicated on the website.

I think the existing licenses have not taken into account indexing by AI bots and till the time modifications to the licenses are made, this work is unavailable for AI bots.

# Credit

I thank the author of the excellent libraries `MailKit` and `MimeKit` [Jeffrey Stedfast](https://github.com/jstedfast) on which this repo is built.
