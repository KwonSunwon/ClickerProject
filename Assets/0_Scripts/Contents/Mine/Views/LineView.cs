using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public static class Slices
{
    private const string PATH = "Art/Mining_Background";
    private static Sprite _source;
    private static readonly Dictionary<int, Sprite> _cache = new();

    private const int SLICE_SIZE = 100;

    static public Sprite GetSprite(int depth)
    {
        if (_cache.TryGetValue(depth, out var sprite)) {
            return sprite;
        }

        _source = Resources.Load<Sprite>(PATH);

        var tex = _source.texture;
        var srcRect = _source.rect;

        int sliceCount = _source.texture.height / SLICE_SIZE;
        int sliceIndex = depth % sliceCount;

        var yFromBottom = srcRect.y + (srcRect.height - SLICE_SIZE) - sliceIndex * SLICE_SIZE;

        Rect cutRect = new(
            0,
            yFromBottom,
            srcRect.width,
            SLICE_SIZE
        );

        sprite = Sprite.Create(tex, cutRect, new Vector2(0.5f, 0.5f));
        _cache.Add(depth, sprite);
        return sprite;
    }
}

public class LineView : MonoBehaviour
{
    private Transform _container;

    [SerializeField] private int _depth;
    public int Depth => _depth;

    private Dictionary<int, RockView> _rocks = null;
    private readonly Dictionary<int, VeinView> _veins = new();

    private Image _image;

    public void Bind(int depth)
    {
        _depth = depth;
        _container = GetComponent<Transform>();

        _image = GetComponent<Image>();
        _image.sprite = Slices.GetSprite(depth);
    }

    public void BuildFrom(
        LineState lineState,
        Func<RockState, RockView> SpawnRock,      // RockView SpawnRock(RockState rock)
        Func<VeinState, VeinView> SpawnVein)      // VeinView SpawnVein(VeinState vein)
    {
        Bind(lineState.Depth);

        if (lineState.Rocks != null) {
            _rocks = new Dictionary<int, RockView>();
            foreach (var rockState in lineState.Rocks) {
                var rockView = SpawnRock(rockState);
                rockView.transform.SetParent(_container, false);
                _rocks.Add(rockView.Id, rockView);
            }
        }
        else {
            foreach (var veinState in lineState.Veins)
                AddVeinView(SpawnVein(veinState));
        }
    }

    public void AddVeinView(VeinView veinView)
    {
        if (_veins.ContainsKey(veinView.Id)) return;
        veinView.transform.SetParent(_container, false);
        _veins.Add(veinView.Id, veinView);
    }

    public void RemoveRock()
    {
        //var cnt = _rocks.Count;
        //for (int i = 1; i <= cnt; i++) {
        //    _rocks.Remove(MakeRockId(_depth, i), out var rock);
        //    if (rock != null) Destroy(rock.gameObject);
        //}
        foreach (var rock in _rocks.Values) {
            if (rock != null) Destroy(rock.gameObject);
        }
        _rocks.Clear();
        _rocks = null;
    }

    public bool TryGetRockView(int rockId, out RockView rockView)
    {
        if (_rocks == null) {
            rockView = null;
            return false;
        }
        return _rocks.TryGetValue(rockId, out rockView);
    }
    public bool TryGetVeinView(int veinId, out VeinView veinView) => _veins.TryGetValue(veinId, out veinView);
}
