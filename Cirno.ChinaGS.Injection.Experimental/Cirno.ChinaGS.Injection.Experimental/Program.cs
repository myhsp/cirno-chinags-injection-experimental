using GS.Unitive.Framework.Core;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Windows.Controls;

namespace Cirno.ChinaGS.Injection.Experimental
{
    public class Program : IAddonActivator
    {
        public IAddonContext addonContext;
        public ControlPanel panel;
        public string panelGuid;
        public bool isPanelOpened;

        public bool debugging = false;

        public void Start(IAddonContext context)
        {
            debugging = true;
            isPanelOpened = false;
            this.addonContext = context;
            this.panel = new ControlPanel(this);

            RegistCardCallBack();
        }

        public void Stop(IAddonContext context)
        {
            
        }

        public void DisplayAdminPanel()
        {
            panelGuid = AddGarnitureControl(panel, 560, 180);
        }

        public void RemoveAdminPanel()
        {
            RemoveGarnitureControl(panelGuid);
        }

        public void RegistCardCallBack()
        {
            string cards = this.addonContext.DictionaryValue("baseConfig", "cards");

            dynamic deviceMan = this.addonContext.GetFirstOrDefaultService("GS.Terminal.DeviceManager",
                "GS.Terminal.DeviceManager.Service.DeviceCallControl");

            deviceMan.RegistCardCallback(
                    new Action<dynamic>(
                        delegate (dynamic data)
                        {
                            string cardId = "ABCDEFFF";
                            cardId = data.OperaDeviceData.Message;

                            string hashString = ComputeHash(cardId);

                            if (debugging)
                            {
                                this.addonContext.Logger.Info("Experiment::" + hashString);
                            }

                            if (cards.Contains(hashString) || cards.Contains(cardId))
                            {
                                if (!isPanelOpened)
                                {
                                    this.DisplayAdminPanel();
                                }
                                else
                                {
                                    this.RemoveAdminPanel();
                                }
                                isPanelOpened = !isPanelOpened;
                            }
                        }
                    )
                );
        }

        public string AddGarnitureControl(UserControl control, double left, double top)
        {
            ///<summary>
            /// 添加用户控件
            ///<returns></returns>
            ///</summary>
            dynamic uiService = this.addonContext.GetFirstOrDefaultService("GS.Terminal.MainShell",
                "GS.Terminal.MainShell.Services.UIService");

            string guid = uiService.AddGarnitureControl(control, top, left);
            return guid;
        }

        public void RemoveGarnitureControl(string guid)
        {
            ///<summary>
            /// 移除用户控件
            ///<returns></returns>
            ///</summary>
            dynamic uiService = this.addonContext.GetFirstOrDefaultService("GS.Terminal.MainShell",
                "GS.Terminal.MainShell.Services.UIService");

            uiService.RemoveGarnitureControl(guid);
        }

        public string ComputeHash(string source)
        {
            string hashString;
            try
            {
                using (SHA256 sha256 = SHA256.Create())
                {
                    byte[] hash = sha256.ComputeHash(Encoding.UTF8.GetBytes(source));
                    hashString = BitConverter.ToString(hash).Replace("-", "").ToLower();
                }
            }
            catch
            {
                hashString = "error";
            }
            return hashString;
        }
    }
}
