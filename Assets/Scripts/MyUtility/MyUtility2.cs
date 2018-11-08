using UnityEditor;
using UnityEngine;
using System.IO;
using System.Collections.Generic;

/// NonResourcesAssetクラスのベースになるコードは下記からダウンロードしました
/// URL https://gist.github.com/kankikuchi/0b1d23f33dd5acd1081f256d15d2a938#file-nonresources-cs
/// URL http://kan-kikuchi.hatenablog.com/entry/NonResources

/// <summary>
/// 自作ユーティリティたち
/// </summary>
namespace MyUtility
{
	/// <summary>
	/// 自作ユーティリティクラス
	/// 注：クラスと複数ファイルに分割していますので注意
	/// </summary>
	public partial class MyUtility
	{
		/// <summary>
		/// Resourcesディレクトリ以外のオブジェクトにアクセスすることができる。Resourcesディレクトリのオブジェクトにもアクセスできる。
		/// 但し、Assetsフォルダ以下かつアセットと認識できるファイルのみ可能。
		/// </summary>
		public static class NonResourcesAsset
		{	
			//=================================================================================
			// 単体ロード
			//=================================================================================
	
			/// <summary>
			/// ファイルのパス(Assetsから、拡張子も含める)と型を設定し、Objectを読み込む。存在しない場合はNullを返す
			/// </summary>
			public static T Load<T>(string path) where T : Object
			{
				return AssetDatabase.LoadAssetAtPath<T>(path);
			}
	
			/// <summary>
			/// ファイルのパス(Assetsから、拡張子も含める)を設定し、Objectを読み込む。存在しない場合はNullを返す
			/// </summary>
			public static Object Load(string path)
			{
				return Load<Object>(path);
			}
	
			//=================================================================================
			// 複数ロード
			//=================================================================================
	
			/// <summary>
			/// ディレクトリのパス(Assetsから)と型を設定し、Objectを読み込む。存在しない場合は空のListを返す
			/// </summary>
			public static List<T> LoadAll<T>(string directoryPath) where T : Object
			{
				List<T> assetList = new List<T>();
	
				//指定したディレクトリに入っている全ファイルを取得(子ディレクトリも含む)
				string[] filePathArray = Directory.GetFiles (directoryPath, "*", SearchOption.AllDirectories);
	
				//取得したファイルの中からアセットだけリストに追加する
				foreach (string filePath in filePathArray)
				{
					T asset = Load<T>(filePath);
	
					if(asset != null)
					{
						assetList.Add (asset);
					}
				}
	
				return assetList;
			}
	
			/// <summary>
			/// ディレクトリのパス(Assetsから)を設定し、Objectを読み込む。存在しない場合は空のListを返す
			/// </summary>
			public static List<Object> LoadAll(string directoryPath)
			{
				return LoadAll<Object> (directoryPath);
			}
		}
	}
}
