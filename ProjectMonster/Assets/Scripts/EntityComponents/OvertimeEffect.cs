﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OvertimeEffect : Effect {

    float currentTime;
    float duration;
    bool done;

    public OvertimeEffect (string name, Color color, float duration = 1f) {
        this.name = name;
        this.color = color;
        this.duration = duration;
    }

    public override bool Update (float frameDelta) {
        currentTime += frameDelta;
        done = (currentTime >= duration);
        return done;
    }

    protected override void OnApply (ref CharacterEntity enemyEntity, out Effect effect) {
        Debug.Log (name + " was applied");
        enemyEntity.speed = enemyEntity.baseSpeed*(1/3f);
        enemyEntity.currentBase = color;
        effect = new OvertimeEffect (name, color, duration);
    }
}
