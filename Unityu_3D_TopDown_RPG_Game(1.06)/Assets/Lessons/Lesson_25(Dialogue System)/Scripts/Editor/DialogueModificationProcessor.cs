using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace Lesson_25
{
    public class DialogueModificationProcessor : UnityEditor.AssetModificationProcessor
    {
        private static AssetMoveResult OnWillMoveAsset(string sourcePath, string destinationPath)
        {
            Dialogue dialogue = AssetDatabase.LoadMainAssetAtPath(sourcePath) as Dialogue; // 소스 경로로부터 대화 객체를 가져옴
            if (dialogue == null)
            {
                return AssetMoveResult.DidNotMove; // 대화 객체가 아닌 경우 파일 이동을 허용하지 않음
            }

            if (Path.GetDirectoryName(sourcePath) != Path.GetDirectoryName(destinationPath))
            {
                return AssetMoveResult.DidNotMove; // 소스 경로와 대상 경로의 디렉터리가 다른 경우 파일 이동을 허용하지 않음
            }

            dialogue.name = Path.GetFileNameWithoutExtension(destinationPath); // 대화 객체의 이름을 대상 경로의 파일 이름으로 설정

            return AssetMoveResult.DidNotMove; // 파일 이동을 완료하고 결과 반환
        }
    }
}
