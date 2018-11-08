using UnityEngine;
using UnityEngine.Assertions;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;


/// <summary>
/// 自作ユーティリティたち
/// </summary>
namespace MyUtility
{
	/// <summary>
	/// 自作ユーティリティクラス
	/// 注：クラスとファイルを分割していますので注意
	/// </summary>
	public partial class MyUtility
	{
		/// <summary>
		/// 指定したprefabからGameObjectをインスタンス化する
		///	GameObject名に"(clone)"が付かないようにしている
		/// </summary>
		/// <param name="prefab">インスタンス化するPrefab</param>
		/// <returns>インスタンス化したGameObject</returns>
		public static GameObject InstantiateGameObjectFromPrefab(GameObject prefab)
		{
			Assert.IsNotNull(prefab);
			GameObject obj = GameObject.Instantiate(prefab) as GameObject;
			obj.name = prefab.name;     // これをやらないとインスタンスに(clone)が付いちゃう 
			return obj;
		}

		/// <summary>
		/// Resourcesフォルダ内のテキストファイルをロードし、文字列化
		/// </summary>
		/// <param name="filePath">ファイルのパス＋ファイル名(Assets/Resources および 拡張子 は含まない事)</param>
		/// <returns></returns>
		public static string LoadResourcesTextFile(string filePath)
		{
			TextAsset tmpText = Resources.Load(filePath) as TextAsset;

			return tmpText.text;
		}

		/// <summary>
		/// Resourcesフォルダ内のjsonテキストファイルをロードし、文字列化
		/// </summary>
		/// <param name="filePath">ファイルのパス＋ファイル名(Assets/Resources および 拡張子 は含まない事)</param>
		/// <param name="compactFlg">軽量化するか(無駄な文字を取り除くか) (true:軽量化する / false:しない)</param>
		/// <returns></returns>
		public static string LoadResourcesJsonTextFile(string filePath, bool compactFlg)
		{
			TextAsset tmpText = Resources.Load(filePath) as TextAsset;
			string jsonStr = tmpText.text;

#if true
			// 注：jsonのデータが壊れたりしていたらまず疑うのはこの処理 
			if (compactFlg)
			{
				string tmpStr = jsonStr.Replace("\n", "");      // 改行コード取り除き 
				tmpStr = tmpStr.Replace("\t", "");              // タブ取り除き 

				// ダブルクォーテーションで囲われていない箇所のスペースを取り除く処理 
				jsonStr = "";
				bool spaceCutFlg = true;

				for (Int32 i = 0; i < tmpStr.Length; i++)
				{
					// データ(文字列)内にダブルクォーテーションが出てきたら
					// 再びダブルクォーテーションが出てくるまでスペースのカットは無効 
					if (tmpStr[i] == '"') { spaceCutFlg ^= true; }    // フラグを反転 

					// カットとコピー処理 
					{
						bool copyFlg = true;

						if (spaceCutFlg)
						{
							if (tmpStr[i] == ' ') { copyFlg = false; }
						}

						if (copyFlg) { jsonStr += tmpStr[i]; }
					}
				}
			}
#endif

			return jsonStr;
		}

		/// <summary>
		/// Byteデータを構造体に格納 
		/// </summary>
		/// <typeparam name="T">構造体の型</typeparam>
		/// <param name="bytes">Byteデータ(値が既に読み込まれている事)</param>
		/// <param name="bufAccessIdx">Bytesの先頭から何バイト目からを格納するか</param>
		/// <returns></returns>
		public static T BytesToStruct<T>(Byte[] bytes, UInt32 bufAccessIdx) where T : struct
		{
			// チェックコード 
			Assert.IsTrue(bytes.Length > 0);
			Int32 stSize = Marshal.SizeOf(typeof(T));
			Int32 accessMax = (Int32)bufAccessIdx + stSize;
			Assert.IsTrue(accessMax <= bytes.Length);

			// アクセス位置からコピー(TODO：ここの処理どうにかならないか？) 
			Byte[] tmpBuf = new byte[bytes.Length - bufAccessIdx];
			Array.Copy(bytes, bufAccessIdx, tmpBuf, 0, tmpBuf.Length);

			// 構造体にコピー 
			GCHandle handle = GCHandle.Alloc(tmpBuf, GCHandleType.Pinned);
			var retData = Marshal.PtrToStructure(handle.AddrOfPinnedObject(), typeof(T));
			handle.Free();

			return (T)retData;
		}

		/// <summary>
		/// Resourcesフォルダ内のAnimationController(mecanim)をロードしてAnimatorにアタッチする
		/// </summary>
		/// <param name="filePath">ファイルのパス＋ファイル名(Assets/Resources および 拡張子 は含まない事)</param>
		/// <param name="animator">アタッチするAnimator</param>
		public static void LoadAndAttachAnimationController(string filePath, Animator animator)
		{
			Assert.IsNotNull(animator);

			RuntimeAnimatorController runtimeAnimController = Resources.Load<RuntimeAnimatorController>(filePath);
			Assert.IsNotNull(runtimeAnimController);

			// アタッチ 
			animator.runtimeAnimatorController = new AnimatorOverrideController() { runtimeAnimatorController = runtimeAnimController };
		}
	}
}
