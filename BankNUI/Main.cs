using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CitizenFX.Core;
using MeneliaAPI.Client;
using static CitizenFX.Core.Native.API;

namespace BankNUI
{
    public class Main : BaseScript
    {
        bool display = false;
        public Main()
        {
            RegisterCommand("bank", new Action<int, List<object>, string>((source, args, raw) =>
            {
                setDisplay(!display);
                Debug.WriteLine("Change from " + !display + " to " + display);
            }), false);

            RegisterNuiCallbackType("main");
            EventHandlers["__cfx_nui:main"] += new Action(main);
            /*EventHandlers["__cfx_nui:main"] += new Action<IDictionary<string, object>, CallbackDelegate>((data, cb) =>
            {
                foreach(KeyValuePair<string, object> test in data)
                {

                }
                // get itemId from the object
                // alternately you could use `dynamic` and rely on the DLR
                if (data.TryGetValue("itemId", out var itemIdObj))
                {
                    cb(new
                    {
                        error = "Item ID not specified!"
                    });

                    return;
                }

                // cast away
                var itemId = (itemIdObj as string) ?? "";

                // same as above
                if (!ItemCache.TryGetValue(itemId, out Item item))
                {
                    cb(new
                    {
                        error = "No such item!"
                    });

                    return;
                }

                cb(item);
            });*/


            RegisterNuiCallbackType("close");
            EventHandlers["__cfx_nui:close"] += new Action(close);
            ClientUtils.GetInstance().InitGetPLayerInfoListener(new Action<string>((json) => {
                SendNuiMessage(json);
                Debug.WriteLine(json);
            }));
        }

        public void main()
        {
            chat("main");
        }

        public void close()
        {
            chat("close");
            setDisplay(false);
        }
        public void setDisplay(bool display)
        {
            if (display)
            {
                ClientUtils.GetInstance().SendLoadPlayerInfo();
            }
            this.display = display;
            SetNuiFocus(display, display);
            SendNuiMessage("{\"type\": \"ui\", \"status\": \"" + display + "\"}");
            Debug.WriteLine("{\"type\": \"ui\", \"status\": \"" + display + "\"}");
        }

        public void chat(String message)
        {
            TriggerEvent("chat:addMessage", new
            {
                color = new[] { 255, 0, 0 },
                multiline = true,
                args = new[] {message}
            });
        }
    }
}
