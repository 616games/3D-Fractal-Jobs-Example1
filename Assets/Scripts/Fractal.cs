using UnityEngine;

public class Fractal : MonoBehaviour
{
    #region --Fields / Properties--
    
    /// <summary>
    /// The total depth of the entire fractal as it relates to the number of children.
    /// </summary>
    [SerializeField, Range(1, 8), Tooltip("Any value above 8 will crash Unity.")]
    private int _depth = 4;

    /// <summary>
    /// Controls the rotation speed of each child fractal.
    /// </summary>
    [SerializeField, Range(0.0f, 50.0f)]
    private float _rotationSpeed = 25f;

    /// <summary>
    /// Cached Transform component.
    /// </summary>
    private Transform _transform;
    
    #endregion
    
    #region --Unity Specific Methods--

    private void Start()
    {
        Init();
    }

    private void Update()
    {
        Rotate();
    }
    
    #endregion
    
    #region --Custom Methods--

    /// <summary>
    /// Initializes variables and caches components.
    /// </summary>
    private void Init()
    {
        if(transform != null && _transform != transform) _transform = transform;
        
        name = "Fractal " + _depth;
        
        InitializeChildFractals();
    }

    /// <summary>
    /// Creates child fractals to match the number of dimensions we want and sets their parent transforms.
    /// </summary>
    private void InitializeChildFractals()
    {
        if (_depth <= 1) return;
        
        Fractal _childA = CreateChildFractal(Vector3.up, Quaternion.identity);
        Fractal _childB = CreateChildFractal(Vector3.right, Quaternion.Euler(0, 0, -90f));
        Fractal _childC = CreateChildFractal(Vector3.left, Quaternion.Euler(0, 0, 90f));
        Fractal _childD = CreateChildFractal(Vector3.forward, Quaternion.Euler(90f, 0, 0));
        Fractal _childE = CreateChildFractal(Vector3.back, Quaternion.Euler(-90f, 0, 0));
        
        _childA._transform.SetParent(_transform, false);
        _childB._transform.SetParent(_transform, false);
        _childC._transform.SetParent(_transform, false);
        _childD._transform.SetParent(_transform, false);
        _childE._transform.SetParent(_transform, false);
    }

    /// <summary>
    /// Used to create a single child fractal.
    /// Formula for number of children:  f(0) = 1, f(n) = 5 * f(n - 1) + 1
    /// </summary>
    private Fractal CreateChildFractal(Vector3 _direction, Quaternion _rotation)
    {
        Fractal _child = Instantiate(this);
        _child._transform = _child.gameObject.transform;
        _child._depth = _depth - 1;
        _child._transform.localScale = .5f * Vector3.one;
        _child._transform.localRotation = _rotation;
        
        //To make the Sphere game objects touch, we need to adjust the position of each Sphere game object to accomodate the reduction in size and its effect on its radius.
        //Sphere game objects start with a radius of .5f, which meant an offset of 1 made them touch.
        //As the child's size has been halved, it's local radius is now .25f, making the offset required for them to touch equal to .75f.
        _child._transform.localPosition = .75f * _direction;

        return _child;
    }
    
    /// <summary>
    /// Rotates each child fractal.
    /// </summary>
    private void Rotate()
    {
        _transform.Rotate(0, _rotationSpeed * Time.deltaTime, 0);
    }
    
    #endregion
    
}
