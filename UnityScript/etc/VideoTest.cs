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
// Render Mode:  Material Override --> 선택한 Mesh Renderer컴포넌트가 붙은 오브젝트 표면에서 영상이 재생됨. Render Texture --> 프로젝트에서 넣은 Render Texture와 연결된 RauImage 컴포넌트가 붙은 UI에서 영상 재생
// Camera Far Plane --> 연결된 카메라의 끝 부분에서 영상 재생(Camera 컴포넌트의 Clipping Planes의 Far의 값에 따라 보이는게 달라짐
// Camera Near Plane --> 연결된 카메라에서 바로 재생 (Camera 컴포넌트의 Viewport Rect에서 X Y W H를 조절해서 카메라를 작게해 미니맵처럼 보이게 할 수 있으며 그 위치도 정할 수 있다)

