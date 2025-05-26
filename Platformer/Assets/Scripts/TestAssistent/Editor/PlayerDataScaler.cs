using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class PlayerDataScaler : EditorWindow
{
    private PlayerData originalData;
    private float scaleFactor = 1f;

    [MenuItem("Tools/Scale PlayerData")]
    public static void ShowWindow()
    {
        GetWindow<PlayerDataScaler>("PlayerData Scaler");
    }

    void OnGUI()
    {
        GUILayout.Label("Масштабирование PlayerData", EditorStyles.boldLabel);
        originalData = (PlayerData)EditorGUILayout.ObjectField("Оригинальный PlayerData", originalData, typeof(PlayerData), false);
        scaleFactor = EditorGUILayout.FloatField("Коэффициент масштаба", scaleFactor);

        if (originalData != null && GUILayout.Button("Создать масштабированную копию"))
        {
            CreateScaledCopy();
        }
    }

    void CreateScaledCopy()
    {
        string path = AssetDatabase.GetAssetPath(originalData);
        string directory = Path.GetDirectoryName(path);
        string newPath = AssetDatabase.GenerateUniqueAssetPath($"{directory}/{originalData.name}_scaled.asset");

        PlayerData newData = ScriptableObject.CreateInstance<PlayerData>();

        // Копируем и масштабируем параметры
        newData.jumpHeight = originalData.jumpHeight * scaleFactor;
        newData.jumpTimeToApex = originalData.jumpTimeToApex;

        newData.fallGravityMult = originalData.fallGravityMult;
        newData.fastFallGravityMult = originalData.fastFallGravityMult;
        newData.maxFallSpeed = originalData.maxFallSpeed * scaleFactor;
        newData.maxFastFallSpeed = originalData.maxFastFallSpeed * scaleFactor;

        newData.runMaxSpeed = originalData.runMaxSpeed * scaleFactor;
        newData.runAcceleration = originalData.runAcceleration * scaleFactor;
        newData.runDecceleration = originalData.runDecceleration * scaleFactor;
        newData.accelInAir = originalData.accelInAir;
        newData.deccelInAir = originalData.deccelInAir;
        newData.doConserveMomentum = originalData.doConserveMomentum;

        newData.jumpCutGravityMult = originalData.jumpCutGravityMult;
        newData.jumpHangGravityMult = originalData.jumpHangGravityMult;
        newData.jumpHangTimeThreshold = originalData.jumpHangTimeThreshold;
        newData.jumpHangAccelerationMult = originalData.jumpHangAccelerationMult;
        newData.jumpHangMaxSpeedMult = originalData.jumpHangMaxSpeedMult;

        newData.wallJumpForce = originalData.wallJumpForce * scaleFactor;
        newData.wallJumpRunLerp = originalData.wallJumpRunLerp;
        newData.wallJumpTime = originalData.wallJumpTime;
        newData.doTurnOnWallJump = originalData.doTurnOnWallJump;

        newData.slideSpeed = originalData.slideSpeed * scaleFactor;
        newData.slideAccel = originalData.slideAccel * scaleFactor;

        newData.coyoteTime = originalData.coyoteTime;
        newData.jumpInputBufferTime = originalData.jumpInputBufferTime;

        newData.dashAmount = originalData.dashAmount;
        newData.dashSpeed = originalData.dashSpeed * scaleFactor;
        newData.dashSleepTime = originalData.dashSleepTime;
        newData.dashAttackTime = originalData.dashAttackTime;
        newData.dashEndTime = originalData.dashEndTime;
        newData.dashEndSpeed = originalData.dashEndSpeed * scaleFactor;
        newData.dashEndRunLerp = originalData.dashEndRunLerp;
        newData.dashRefillTime = originalData.dashRefillTime;
        newData.dashInputBufferTime = originalData.dashInputBufferTime;

        // Пересчёт зависимых переменных
        newData.OnValidate();

        // Сохраняем как новый asset
        AssetDatabase.CreateAsset(newData, newPath);
        AssetDatabase.SaveAssets();

        EditorUtility.FocusProjectWindow();
        Selection.activeObject = newData;

        Debug.Log($"Создан новый PlayerData с масштабом {scaleFactor} по пути: {newPath}");
    }
}
