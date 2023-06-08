﻿using MazeRunner.Content;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MazeRunner.Managers;

public static class SoundManager
{
    private static readonly SoundEffectInstance _buttonPressed;

    private static readonly SoundEffectInstance _radioButtonPressed;
    
    private static readonly SoundEffectInstance _keyCollected;

    private static readonly SoundEffectInstance _foodEaten;

    static SoundManager()
    {
        _buttonPressed = Sounds.Buttons.Button.CreateInstance();
        _radioButtonPressed = Sounds.Buttons.RadioButton.CreateInstance();
        _keyCollected = Sounds.Notifiers.KeyCollected.CreateInstance();
        _foodEaten = Sounds.Notifiers.KeyCollected.CreateInstance();
    }

    public static void PlayButtonPressedSound()
    {
        _buttonPressed.Play();
    }

    public static void PlayRadioButtonPressedSound()
    {
        _radioButtonPressed.Play();
    }

    public static void PlayKeyCollectedSound()
    {
        _keyCollected.Play();
    }

    public static void PlayFoodEatenSound()
    {
        _foodEaten.Play();
    }
}