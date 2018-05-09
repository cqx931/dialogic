﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Dialogic;
using MessagePack;
using Tendar;

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
            testfile += "../../../../dialogic/data/gscript.gs";
            Console.WriteLine(testfile);
            FileInfo file = new FileInfo(srcpath + "/data/gscript.gs");
            new MockGameEngine(new FileInfo(testfile)).Run();
        }

        public static string srcpath = "../../../dialogic";
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

        bool interrupted = false;
        string evtText, evtType, evtActor;
        string[] evtOpts;

        /// <summary>
        /// Create an engine from a script file or folder script files
        /// </summary>
        /// <param name="fileOrFolder">File or folder.</param>
        public MockGameEngine(FileInfo fileOrFolder)
        {
            var saveFile = new FileInfo("./runtime.ser");

            ISerializer serializer = new SerializerMessagePack();
            ChatRuntime tmp = new ChatRuntime(Tendar.AppConfig.Actors);
            tmp.ParseFile(fileOrFolder);
            tmp.Save(serializer, saveFile);

            dialogic = ChatRuntime.Create(serializer, saveFile, AppConfig.Actors);
            dialogic.Run();
        }

        /// <summary>
        /// Start the Run loop for the engine
        /// </summary>
        public void Run()
        {
            var now = Util.Millis();

            // a 'Tap' event
            if (true) Timers.SetTimeout(Util.Rand(2000, 9999), () =>
             {
                 interrupted = true;
                 Console.WriteLine("\n<user-event#tap>" +
                     " after " + Util.Millis(now) + "ms\n");

                 gameEvent = new UserEvent("Tap");
             });

            var types = new[] { "critic", "shake", "tap" };
            var count = 0;

            // a 'Resume' event
            if (false) Timers.SetInterval(1000, () =>
            {
                interrupted = true;
                var data = "{!!type = TYPE,!stage = CORE}";
                data = data.Replace("TYPE", types[++count % 3]);

                Console.WriteLine("\n<resume-event#" + data + ">" +
                    " after " + Util.Millis(now) + "ms\n");

                gameEvent = new ResumeEvent(data);
            });

            while (true)
            {
                Thread.Sleep(30);
                IUpdateEvent ue = dialogic.Update(globals, ref gameEvent);
                if (ue != null) HandleEvent(ref ue);
            }
        }

        internal void RunInLoop() // repeated events
        {
            int ts = 0;
            int count = 0;
            while (true)
            {
                Thread.Sleep(30);
                IUpdateEvent ue = dialogic.Update(globals, ref gameEvent);
                if (ue != null) HandleEvent(ref ue);
                //if (Util.Millis(ts) > 1000)
                //{

                //    ts = Util.Millis();
                //    if (++count < 5)
                //    {
                //        FuzzySearch.DBUG = count == 4;
                //        gameEvent = new ResumeEvent("{type = test}");

                //    }
                //}
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

        // Implement ISerializer and then instance to ChatRuntime methods...
        private class SerializerMessagePack : ISerializer
        {
            IFormatterResolver ifr = MessagePack.Resolvers.
                ContractlessStandardResolverAllowPrivate.Instance;

            public byte[] ToBytes(ChatRuntime rt)
            {
                return MessagePackSerializer.Serialize<Snapshot>(Snapshot.Create(rt), ifr);
            }

            public void FromBytes(ChatRuntime rt, byte[] bytes)
            {
                MessagePackSerializer.Deserialize<Snapshot>(bytes, ifr).Update(rt);
            }

            public string ToJSON(ChatRuntime rt)
            {
                return MessagePackSerializer.ToJson(ToBytes(rt), ifr);
            }
        }
    }
}
