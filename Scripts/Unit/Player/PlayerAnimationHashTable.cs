using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationHashTable
{
    private static readonly Dictionary<int, AnimationClip> HashToClip = new();

    public static void Initialize(RuntimeAnimatorController controller)
    {
        HashToClip.Clear();

        foreach (var clip in controller.animationClips)
        {
            int hash = Animator.StringToHash(clip.name);
            if (!HashToClip.ContainsKey(hash))
                HashToClip.Add(hash, clip);
        }
    }

    public static AnimationClip GetClipByHash(int hash)
    {
        return HashToClip.TryGetValue(hash, out var clip) ? clip : null;
    }
}
