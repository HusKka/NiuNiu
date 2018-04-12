﻿using ETModel;

namespace ETHotfix
{
    /// <summary>
    /// 用于Session断开时触发下线
    /// </summary>
    public class SessionOfflineComponent : Component
    {
        public override void Dispose()
        {
            if (this.IsDisposed)
            {
                return;
            }

            base.Dispose();

            //移除连接组件
            ETModel.Game.Scene.RemoveComponent<SessionComponent>();
            Game.Scene.RemoveComponent<SessionWrapComponent>();

            //释放本地玩家对象
            PlayerComponent playerComponent = ETModel.Game.Scene.GetComponent<PlayerComponent>();
            if (playerComponent.MyPlayer != null)
            {
                playerComponent.MyPlayer.Dispose();
                playerComponent.MyPlayer = null;
            }

            UIComponent uiComponent = Game.Scene.GetComponent<UIComponent>();
            //游戏关闭，不用回到登录界面
            if(uiComponent == null || uiComponent.IsDisposed)
            {
                return;
            }

            //UI uiLogin = uiComponent.Create(UIType.UILogin);
            //uiLogin.GetComponent<UILoginComponent>().SetPrompt("连接断开");

            if (uiComponent.Get(UIType.UILobby) != null)
            {
                uiComponent.Remove(UIType.UILobby);
            }
            else if (uiComponent.Get(UIType.UIRoom) != null)
            {
                uiComponent.Remove(UIType.UIRoom);
            }
        }
    }
}
