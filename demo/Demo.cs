﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Dialogic;
using Client;

namespace runner
{
    /// <summary>
    /// Entry point to run the test program (a mock game engine)
    /// </summary>
    class Demo
    {
        public static void Main(string[] args)
        {
            var testfile = AppDomain.CurrentDomain.BaseDirectory;
            testfile += "../../../../dialogic/data/gscript-loop.gs";
            new MockGameEngine(new FileInfo(testfile)).Run();
        }

        public static void Profiling(string[] args)
        {
            ChatRuntime.VERIFY_UNIQUE_CHAT_LABELS = false;

            AppConfig config = AppConfig.TAC;
            ISerializer serializer = new SerializerMessagePack();

            ChatRuntime rtOut, rtIn;
            byte[] bytes = null;
            int iterations = 10;

            var testfile = AppDomain.CurrentDomain.BaseDirectory;
            testfile += "../../../../dialogic/data/allchats.gs";

            rtIn = new ChatRuntime(config);

            var watch = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < iterations; i++)
            {
                rtIn.ParseFile(new FileInfo(testfile));
            }
            var numChats = rtIn.Chats().Count;
            watch.Stop(); Console.WriteLine("Parsed " + numChats
                + " chats in " + watch.ElapsedMilliseconds / 1000.0 + "s");

            for (int i = 0; i < iterations; i++)
            {
                watch = System.Diagnostics.Stopwatch.StartNew();
                bytes = serializer.ToBytes(rtIn);
                watch.Stop();
                Console.WriteLine("Serialize #" + i + ": "
                    + watch.ElapsedMilliseconds / 1000.0 + "s");
            }

            for (int i = 0; i < iterations; i++)
            {
                watch = System.Diagnostics.Stopwatch.StartNew();
                rtOut = ChatRuntime.Create(serializer, bytes, config);
                watch.Stop(); Console.WriteLine("Deserialize #" + i + ": "
                    + watch.ElapsedMilliseconds / 1000.0 + "s");
            }
        }
    }

    /// <summary>
    /// A simple fake game engine that calls dialogic's Update function
    /// 30 times (or so) per second
    /// </summary>
    public class MockGameEngine
    {
        static Dictionary<string, object> globals =
            new Dictionary<string, object>() {
                { "emotion", "special" },
                { "place", "my tank" },
                { "Happy", "HappyFlip" },
                { "verb", "play" },
                { "neg", "(nah|no|nope)" },
                { "var3", 2 }
            };

        ChatRuntime dialogic;
        EventArgs gameEvent;
        ISerializer serializer;

        bool interrupted = false;
        string evtText, evtType, evtActor;
        string[] evtOpts;

        /// <summary>
        /// Create an engine from a script file or folder script files
        /// </summary>
        /// <param name="fileOrFolder">File or folder.</param>
        public MockGameEngine(FileInfo fileOrFolder)
        {
            var config = AppConfig.TAC;
            var saveFile = new FileInfo("./runtime.ser");

            ChatRuntime tmp = new ChatRuntime(config);
            tmp.ParseFile(fileOrFolder);

            serializer = new SerializerMessagePack();
            tmp.Save(serializer, saveFile);

            dialogic = ChatRuntime.Create(serializer, saveFile, config);
            dialogic.Run();
        }

        /// <summary>
        /// Start the Run loop for the engine
        /// </summary>
        public void Run()
        {
            TestEvents(Util.Millis());

            while (true)
            {
                Thread.Sleep(30);
                IUpdateEvent ue = dialogic.Update(globals, ref gameEvent);
                if (ue != null) HandleEvent(ref ue);
            }
        }

        public void TestEvents(int now)
        {
            // a 'Tap' event

            /*Timers.SetTimeout(Util.Rand(2000, 9999), () =>
             {
                 Console.WriteLine("\n<user-event#tap>" +
                     " after " + Util.Millis(now) + "ms\n");

                 gameEvent = new UserEvent("Tap");
             });*/


            // a 'Resume' event

            /*var count = 0;
            var types = new[] { "critic", "shake", "tap" };
            Timers.SetInterval(1000, () =>
            {

                interrupted = true;
                var data = "{!!type = TYPE,!stage = CORE}";
                data = data.Replace("TYPE", types[++count % 3]);

                Console.WriteLine("\n<resume-event#" + data + ">" +
                    " after " + Util.Millis(now) + "ms\n");

                gameEvent = new ResumeEvent(data);
            });*/

            // a 'Save' event

            /*Timers.SetTimeout(Util.Rand(4000, 6000), () =>
            {
                var file = AppDomain.CurrentDomain.BaseDirectory;
                file += Util.EpochMs() + ".ser";

                Console.WriteLine("\n<save-event#file=" + file + ">\n");

                gameEvent = new SaveEvent(serializer, new FileInfo(file));
            });*/

            /* pending ----------------------------

                // a 'Merge' event
                if (false) Timers.SetTimeout(Util.Rand(4000, 6000), () =>
                {
                    Console.WriteLine("\n<new-chat-event#chats>\n");
                    var file = AppDomain.CurrentDomain.BaseDirectory;
                    file += Util.EpochMs() + ".ser";
                    gameEvent = new MergeEvent(serializer, new FileInfo(file));
                });

                // an 'AppendChats' event
                if (false) Timers.SetTimeout(Util.Rand(4000, 6000), () =>
                {
                    Console.WriteLine("\n<new-chat-event#chats>\n");

                    var runtime = new ChatRuntime(Client.AppConfig.TAC);
                    runtime.ParseText(string.Join('\n', new[] {
                        "CHAT GScriptTest {type=a,stage=b}",
                        "*** Welcome to my updated world!!!"
                    }));
                    gameEvent = new LoadChatsEvent(runtime.Chats());
                });
            */
        }

        internal void RunInLoop() // repeated events
        {
            //int ts = 0;
            //int count = 0;
            while (true)
            {
                Thread.Sleep(30);
                IUpdateEvent ue = dialogic.Update(globals, ref gameEvent);
                if (ue != null) HandleEvent(ref ue);
                //if (Util.Millis(ts) > 1000) {
                //    ts = Util.Millis();
                //    if (++count < 5) {
                //        FuzzySearch.DBUG = count == 4;
                //        gameEvent = new ResumeEvent("{type = test}");
                //    }}
            }
        }

        ////////////////////////////////////////////////////////////////////////   


        /// <summary>
        /// Prints our the various commands with useful debugging info
        /// </summary>
        private void HandleEvent(ref IUpdateEvent ue)
        {
            interrupted = false;

            evtText = ue.Text();
            evtType = ue.Type();
            evtActor = ue.Actor();

            //evtText = "["+evtActor + "] " + evtText;

            ue.RemoveKeys(Meta.TEXT, Meta.TYPE, Meta.ACTOR);

            switch (evtType)
            {
                case "Say":
                    evtText = evtText + " " + ue.Data().Stringify();
                    break;

                case "Ask":
                    DoPrompt(ue);
                    SendRandomResponse(ue);
                    break;

                case "Wait":
                    var now = Util.Millis();

                    Timers.SetTimeout(3000, () =>
                    {
                        if (!interrupted)
                        {
                            Console.WriteLine("\n<resume-event#>" +
                                " after " + Util.Millis(now) + "ms\n");

                            // send ResumeEvent after 5 sec
                            // (), (#Game), or ({type=a,stage=b,last=true})
                            gameEvent = new ResumeEvent();
                        }
                    });

                    evtText = ("(" + (evtType + " " +
                        ue.Data().Stringify()).Trim() + ")");
                    break;

                default:
                    evtText = ("(" + evtType + ": " + (evtText + " "
                        + ue.Data().Stringify()).Trim() + ")");
                    break;
            }

            Console.WriteLine(evtText);

            ue = null;  // dispose event 
        }

        private void DoPrompt(IUpdateEvent ue)
        {
            evtOpts = ue.Get(Meta.OPTS).Split('\n');

            ue.RemoveKeys(Meta.TEXT, Meta.TYPE, Meta.OPTS);

            // add any meta tags
            evtText = evtText + " " + ue.Data().Stringify();

            // add the options
            for (int i = 0; i < evtOpts.Length; i++)
            {
                evtText += "\n  (" + i + ") " + evtOpts[i];
            }
        }

        private void SendRandomResponse(IUpdateEvent ue)
        {
            double timeout = ue.GetDouble(Meta.TIMEOUT, -1);
            if (timeout > -1)
            {
                var delay = Util.ToMillis(Util.Rand(timeout / 3, timeout));
                Timers.SetTimeout(delay, () =>
                {
                    // choice a valid response, or -1 for no response
                    int choice = Util.Rand(evtOpts.Length + 1) - 1;
                    Console.WriteLine("\n<choice-index#" + choice + "> after " + delay + "ms\n");
                    gameEvent = new ChoiceEvent(choice);
                });
            }
        }
    }
}
