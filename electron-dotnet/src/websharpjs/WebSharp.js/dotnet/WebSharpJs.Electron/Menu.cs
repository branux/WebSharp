﻿using System.Threading.Tasks;
using WebSharpJs.NodeJS;
using WebSharpJs.Script;

namespace WebSharpJs.Electron
{
    public class Menu : NodeJsObject
    {
        protected override string ScriptProxy =>
                        @"function () { 

                            if (options && options.length > 0)
                            {
                                return Menu.buildFromTemplate(options);
                            }
                            else
                                return new Menu(); 
                        }()";

        protected override string Requires => @"const {Menu} = websharpjs.IsRenderer() ? require('electron').remote : require('electron');";

        // Save off the ScriptObjectProxy implementation to cut down on bridge calls.
        static NodeObjectProxy scriptProxy;

        public static async Task<Menu> Create()
        {
            var proxy = new Menu();
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize();
            return proxy;

        }

        public static async Task<Menu> BuildFromTemplate(MenuItemOptions[] template)
        {
            var proxy = new Menu();
            if (scriptProxy != null)
                proxy.ScriptObjectProxy = scriptProxy;

            scriptProxy = await proxy.Initialize(template);
            return proxy;

        }


        [ScriptableMember(ScriptAlias = "append")]
        public async Task<object> Append(MenuItem menuItem)
        {
            return await Invoke<object>("Append", menuItem);
        }

        [ScriptableMember(ScriptAlias = "insert")]
        public async Task<object> Insert(int pos, MenuItem menuItem)
        {
            return await Invoke<object>("Insert", pos, menuItem);
        }

        public async Task<ScriptObjectCollection<MenuItem>> Items()
        {
            return await GetProperty<ScriptObjectCollection<MenuItem>>("items"); ;
        }

        public async Task<object> Popup()
        {
            return await Invoke<object>("popup");
        }
    }
}
