﻿using FMODUnity;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//============================================
//
// ParameterByAltitude
// 플레이어의 고도에 따라 FMOD의 파라미터 값을 바꿉니다.
//
//============================================

public class ParameterByAltitude : MonoBehaviour
{
    [SerializeField] string ParameterName;      // FMOD 파라미터 이름
    [SerializeField] float heightStart;         // 고도 최소값
    [SerializeField] float heightEnd;           // 고도 최대값
    [SerializeField] bool isGlobalParam;        // 해당 파라미터가 Global값일 경우 true, StudioEventEmitter 컴포넌트랑 붙여쓸경우 false

    Transform playerTF;
    StudioEventEmitter sound;

    bool hasEventComp = false;

    private void OnEnable()
    {
        PlayerCore player = FindFirstObjectByType<PlayerCore>();
        hasEventComp = TryGetComponent<StudioEventEmitter>(out sound);
        if(player != null)
        {
            playerTF = player.transform;
        }
    }

    private void Update()
    {
        if (playerTF != null)
        {
            if (isGlobalParam)
            {
                float value = 0f;
                if (RuntimeManager.StudioSystem.getParameterByName(ParameterName, out value) == FMOD.RESULT.OK)
                    RuntimeManager.StudioSystem.setParameterByName(ParameterName, Mathf.InverseLerp(heightStart, heightEnd, playerTF.position.y));
            }
            else
            {
                if (hasEventComp)
                {
                    sound.EventInstance.setParameterByName(ParameterName, Mathf.InverseLerp(heightStart, heightEnd, playerTF.position.y));
                }
            }
        }
    }
}
