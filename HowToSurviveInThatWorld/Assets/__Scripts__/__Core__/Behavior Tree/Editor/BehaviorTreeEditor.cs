using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor.Callbacks;

public class BehaviorTreeEditor : EditorWindow
{
    /*===========================================================================================================*/
    
    private BehaviorTreeView _treeView;
    private InspectorView _inspectorView;
    
    /*===========================================================================================================*/
    
    [MenuItem("BehaviorTree/BehaviorTree Editor")]
    public static void OpenWindow()
    {
        BehaviorTreeEditor wnd = GetWindow<BehaviorTreeEditor>();
        wnd.titleContent = new GUIContent("BehaviorTreeEditor");
    }

    // BT SO 더블클릭, Editor Open
    [OnOpenAsset]
    public static bool OnOpenAsset(int instanceId, int line)
    {
        if (Selection.activeObject is BehaviorTree)
        {
            OpenWindow();
            
            return true;
        }

        return false;
    }
    

    public void CreateGUI()
    {
        // Each editor window contains a root VisualElement object
        VisualElement root = rootVisualElement;

        // Instantiate UXML
        var visualTree = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>("Assets/__Scripts__/__Core__/Behavior Tree/UI Builders/BehaviorTreeEditor.uxml");
        visualTree.CloneTree(root);
        
        // Allocate StyleSheet 
        var styleSheet = AssetDatabase.LoadAssetAtPath<StyleSheet>("Assets/__Scripts__/__Core__/Behavior Tree/UI Builders/BehaviorTreeEditor.uss");
        root.styleSheets.Add(styleSheet);

        _treeView = root.Q<BehaviorTreeView>();
        _inspectorView = root.Q<InspectorView>();
        _treeView.OnNodeSelected = OnNodeSelectionChanged;
        
        OnSelectionChange();
    }

    private void OnEnable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
        EditorApplication.playModeStateChanged += OnPlayModeStateChanged;
    }

    private void OnDisable()
    {
        EditorApplication.playModeStateChanged -= OnPlayModeStateChanged;
    }
    
    // 플레이 모드 상태가 변경될 때 호출되는 메서드
    private void OnPlayModeStateChanged(PlayModeStateChange obj)
    {
        switch (obj)
        {
            case PlayModeStateChange.EnteredEditMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingEditMode:
                break;
            case PlayModeStateChange.EnteredPlayMode:
                OnSelectionChange();
                break;
            case PlayModeStateChange.ExitingPlayMode:
                break;
        }
    }

    // 선택시 변경 이벤트 함수
    private void OnSelectionChange()
    {
        BehaviorTree tree = Selection.activeObject as BehaviorTree;

        if (_treeView == null)
            return;

        if (!tree)
        {
            if (Selection.activeGameObject) // 화성화된 게임 오브젝트가 null이 아니라면
            {
                BehaviorTreeRunner runner = Selection.activeGameObject.GetComponent<BehaviorTreeRunner>();
                if (runner)
                    tree = runner.tree;
            }
        }

        if (Application.isPlaying)
        {
            if (tree)
                _treeView.PopulateView(tree);
        }
        else
        {
            if (tree && AssetDatabase.CanOpenAssetInEditor(tree.GetInstanceID()))
            {
                _treeView.PopulateView(tree);
            }
        }
    }

    // 노드가 선택될 때 호출, Inspector View를 입력받은 node 정보로 업데이트.
    private void OnNodeSelectionChanged(NodeView node)
    {
        _inspectorView.UpdateSelection(node);
    }
}
