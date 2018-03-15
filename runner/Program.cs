﻿using System;
using System.Collections.Generic;
using System.Threading;
using Dialogic;

namespace runner
{
    class Program
    {
        public static void Main(string[] args)
        {
            new MockGameEngine(srcpath + "/data/gscript.gs").Run();
        }

        public static string srcpath = "../../../dialogic";
    }

    public class MockGameEngine
    {
        public static Dictionary<string, object> globals =
            new Dictionary<string, object>() {
                { "emotion", "special" },
                { "place", "my tank" },
                { "Happy", "HappyFlip" },
                { "verb", "play" },
                { "neg", "(nah|no|nope)" },
                { "var3", 2 }
            };

        private readonly ChatRuntime runtime;
        private GameEvent gameEvent;
        string diaText, diaType;
        string[] diaOpts;

        public MockGameEngine(string fileOrFolder)
        {
            List<Chat> chats = ChatParser.ParseFile(fileOrFolder);
            runtime = new ChatRuntime(chats);
            runtime.Run();
        }

        public void Run()
        {
            Console.WriteLine();
            while (true)
            {
                Thread.Sleep(30);
                IUpdateEvent ue = runtime.Update(globals, ref gameEvent);
                if (ue != null) HandleEvent(ref ue);
            }
        }

        private void HandleEvent(ref IUpdateEvent ue)
        {
            diaText = ue.Text();
            diaType = ue.Type();

            ue.RemoveKeys(Meta.TEXT, Meta.TYPE);

            switch (diaType)
            {
                case "Say":
                    diaText += " " + Util.Stringify(ue.Data());
                    break;

                case "Ask":
                    DoPrompt(ue);
                    SendRandomResponse(ue);
                    break;

                case "Wait":
                    Console.WriteLine("abput to start timer");

                    Timers.SetTimeout(5000, () =>
                    {
                        Console.WriteLine("create resume-event");
                        gameEvent = new ResumeEvent();
                    });
                    diaText = ("(" + diaType + ": " + diaText + " "
                        + Util.Stringify(ue.Data())).Trim() + ")";
                    break;

                default:
                    diaText = ("(" + diaType + ": " + diaText + " "
                        + Util.Stringify(ue.Data())).Trim() + ")";
                    break;
            }

            Console.WriteLine(diaText);

            ue = null;  // dispose event 
        }

        private void DoPrompt(IUpdateEvent ge)
        {
            diaOpts = ge.Get(Meta.OPTS).Split('\n');

            ge.RemoveKeys(Meta.TEXT, Meta.TYPE, Meta.OPTS);
            diaText += " " + Util.Stringify(ge.Data()); // show meta

            for (int i = 0; i < diaOpts.Length; i++)
            {
                diaText += "\n  (" + i + ") " + diaOpts[i];
            }
        }

        private void SendRandomResponse(IUpdateEvent ge)
        {
            int timeout = ge.GetInt(Meta.TIMEOUT, -1);
            if (timeout > -1)
            {
                var delay = Util.Rand(timeout / 3, timeout);
                Timers.SetTimeout(delay, () =>
                {
                    // choice a valid response, or -1 for no response
                    int choice = Util.Rand(diaOpts.Length + 1) - 1;
                    Console.WriteLine("\n<choice-index#" + choice + "> after "+delay+"ms\n");
                    gameEvent = new ChoiceEvent(choice);
                });
            }
        }

    }
}
