#if UNITY_EDITOR
using UnityEngine;
using UnityEditor;

namespace ASPageView
{
    [CustomEditor(typeof(PageView))]
    public class PageViewEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            PageView pageView = target as PageView;

			pageView.doNotAllowBackwardNavigation = EditorGUILayout.Toggle ("Do Not Allow Backward Navigation", pageView.doNotAllowBackwardNavigation);
            pageView.pageIndicator = (RectTransform)EditorGUILayout.ObjectField("Page Indicator", pageView.pageIndicator, typeof(RectTransform), true);
            pageView.viewport = (RectTransform)EditorGUILayout.ObjectField("Viewport", pageView.viewport, typeof(RectTransform), true);
            pageView.content = (RectTransform)EditorGUILayout.ObjectField("Pages", pageView.content, typeof(RectTransform), true);

            EditorUtility.SetDirty(pageView);
        }
    }
}
#endif
