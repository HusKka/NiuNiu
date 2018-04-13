﻿using System;
using System.Threading;
using UnityEngine;

namespace ETModel
{
	public class Init : MonoBehaviour
	{
		private readonly OneThreadSynchronizationContext contex = new OneThreadSynchronizationContext();

		private async void Start()
		{
			try
			{
				if (Application.unityVersion != "2017.1.0p5")
				{
					Log.Warning($"当前版本:{Application.unityVersion}, 最好使用运行指南推荐版本!");
				}

				SynchronizationContext.SetSynchronizationContext(this.contex);

				DontDestroyOnLoad(gameObject);
				Game.EventSystem.Add(DLLType.Model, typeof(Init).Assembly);

				Game.Scene.AddComponent<GlobalConfigComponent>();
				Game.Scene.AddComponent<NetOuterComponent>();
				Game.Scene.AddComponent<ResourcesComponent>();
				Game.Scene.AddComponent<BehaviorTreeComponent>();
				Game.Scene.AddComponent<PlayerComponent>();
				Game.Scene.AddComponent<UnitComponent>();
				Game.Scene.AddComponent<ClientFrameComponent>();
				Game.Scene.AddComponent<UIComponent>();

                //斗地主客户端自定义全局组件
                //用于保存玩家本地数据
                Game.Scene.AddComponent<ClientComponent>();

                // 下载ab包
                await BundleHelper.DownloadBundle();

				Game.Hotfix.LoadHotfixAssembly();

				// 加载配置
				Game.Scene.GetComponent<ResourcesComponent>().LoadBundle("config.unity3d");
				Game.Scene.AddComponent<ConfigComponent>();
				Game.Scene.GetComponent<ResourcesComponent>().UnloadBundle("config.unity3d");
				Game.Scene.AddComponent<OpcodeTypeComponent>();
				Game.Scene.AddComponent<MessageDispatherComponent>();

				Game.Hotfix.GotoHotfix();

				Game.EventSystem.Run(EventIdType.TestHotfixSubscribMonoEvent, "TestHotfixSubscribMonoEvent");
			}
			catch (Exception e)
			{
				Log.Error(e);
			}
		}

		private void Update()
		{
			this.contex.Update();
			Game.Hotfix.Update?.Invoke();
			Game.EventSystem.Update();
		}

		private void LateUpdate()
		{
			Game.Hotfix.LateUpdate?.Invoke();
			Game.EventSystem.LateUpdate();
		}

		private void OnApplicationQuit()
		{
			Game.Hotfix.OnApplicationQuit?.Invoke();
			Game.Close();
		}
	}
}