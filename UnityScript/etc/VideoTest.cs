using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Video;

public class VideoTest : MonoBehaviour
{
    public VideoPlayer vp;

    private void Start()
    {
        vp.clip = Resources.Load<VideoClip>("vc1");
        vp.playbackSpeed = 2;
        vp.Play();
    }
}

//VideoPlayer Component
// Render Mode:  Material Override --> ������ Mesh Renderer������Ʈ�� ���� ������Ʈ ǥ�鿡�� ������ �����. Render Texture --> ������Ʈ���� ���� Render Texture�� ����� RauImage ������Ʈ�� ���� UI���� ���� ���
// Camera Far Plane --> ����� ī�޶��� �� �κп��� ���� ���(Camera ������Ʈ�� Clipping Planes�� Far�� ���� ���� ���̴°� �޶���
// Camera Near Plane --> ����� ī�޶󿡼� �ٷ� ��� (Camera ������Ʈ�� Viewport Rect���� X Y W H�� �����ؼ� ī�޶� �۰��� �̴ϸ�ó�� ���̰� �� �� ������ �� ��ġ�� ���� �� �ִ�)

