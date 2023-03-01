using System;
using System.IO;
using System.Net;
using System.Threading;
using System.Diagnostics;

using MailKit.Net.Imap;
using MailKit.Search;
using MailKit;
using MimeKit;

namespace IMAPTransfer {
    class Program
    {
        private static void ConnectClients(ImapClient client, ImapClient client2, CancellationTokenSource cancel) {
            client.Connect ("imap.source_server.com", 993, true, cancel.Token);
            client.AuthenticationMechanisms.Remove ("XOAUTH");
            client.Authenticate ("<email addr>", "<password>", cancel.Token);

            client2.Connect ("imap.target_server.com", 993, true, cancel.Token);
            client2.AuthenticationMechanisms.Remove ("XOAUTH");
            client2.Authenticate ("<email addr>", "<password>", cancel.Token);
        }

        public static void Main (string[] args)
        {
            bool isComplete = false;
            int lastFolder = 0;
            int lastIdProcessed = 0;
            IMailFolder box = null;
            IMailFolder box2 = null;
            string[] folders = {"Inbox", "Sent"};
            using (var cancel = new CancellationTokenSource ()) {
                while (!isComplete) {
                    try {
                        using (var client = new ImapClient ()) {
                            using (var client2 = new ImapClient ()) {
                                try {
                                    bool toReconnect = false;
                                    ConnectClients(client, client2, cancel);
                                    int folder_ctr = 0;
                                    foreach (var folder in folders) {
                                        if (folder_ctr++ < lastFolder)
                                            continue;
                                        lastFolder = folder_ctr;
                                        box = client.GetFolder(folder);
                                        box.Open (FolderAccess.ReadOnly, cancel.Token);
                                        box2 = client2.GetFolder(folder.Equals("Archive") ? "Archive1" : folder);
                                        box2.Open (FolderAccess.ReadOnly, cancel.Token);
                                        //Directory.CreateDirectory(folder);
                                        Console.WriteLine ("Total messages: {0}, {1}", folder, box.Count);
                                        Console.WriteLine ("Total messages: {0}, {1}", folder, box2.Count);
                                        var items = box.Fetch (lastIdProcessed + 1, -1, MessageSummaryItems.UniqueId | MessageSummaryItems.InternalDate | MessageSummaryItems.Flags);
                                        int counter = lastIdProcessed + 1;
                                        foreach (var item in items) {
                                            if (item.Flags.Value.HasFlag (MessageFlags.Deleted))
                                                continue;
                                            try {
                                                var message = box.GetMessage(item.UniqueId);
                                                box2.Append (message, item.Flags.Value, item.InternalDate.Value);
                                                Console.WriteLine(counter + " " + folder + " " + message.Subject);
                                            } catch (Exception e4) {
                                                toReconnect = true;
                                                try {
                                                    client.Disconnect (true, cancel.Token);
                                                    client2.Disconnect (true, cancel.Token);
                                                } catch (Exception e6) {
                                                    Console.WriteLine(e6.ToString());
                                                }
                                                Thread.Sleep(10000);
                                                break;
                                            }
                                            lastIdProcessed = counter;
                                            counter++;
                                            //string si = "" + i;
                                            //var filename = folder + "/" + si.PadLeft(6, '0') + ".eml";
                                            //Console.WriteLine(filename);
                                            //message.WriteTo(filename);
                                            //var p = Process.Start("zip", " -q \"" + folder + ".zip\" \"" + filename + "\"");
                                            //p.WaitForExit();
                                            //File.Delete(filename);
                                        }
                                        try {
                                            box.Close();
                                            box2.Close();
                                            box = null;
                                            box2 = null;
                                        } catch (Exception e5) {
                                            Console.WriteLine(e5.ToString());
                                        }
                                        lastIdProcessed = -1;
                                        if (toReconnect)
                                            break;
                                    }
                                    /*
                                    // let's try searching for some messages...
                                    var query = SearchQuery.DeliveredAfter (DateTime.Parse ("2013-01-12"))
                                        .And (SearchQuery.SubjectContains ("MailKit"))
                                        .And (SearchQuery.Seen);

                                    foreach (var uid in inbox.Search (query, cancel.Token)) {
                                        var message = inbox.GetMessage (uid, cancel.Token);
                                        Console.WriteLine ("[match] {0}: {1}", uid, message.Subject);
                                    }
                                    */
                                    client.Disconnect (true, cancel.Token);
                                    client2.Disconnect (true, cancel.Token);
                                    isComplete = true;
                                } catch (Exception e3) {
                                    Console.WriteLine(e3.ToString());
                                    try {
                                        client.Disconnect (true, cancel.Token);
                                        client2.Disconnect (true, cancel.Token);
                                    } catch (Exception e7) {
                                        Console.WriteLine(e7.ToString());
                                    }
                                } 
                            }
                        }
                    } catch (Exception e) {
                        Console.WriteLine(e.ToString());
                        if (box != null)
                            box.Close();
                        if (box2 != null)
                            box2.Close();
                        Thread.Sleep(10000);
                    }
                }
            }
        }
    }
}
