using SharpAstrology.Interfaces;
using SharpAstrology.Enums;
using SharpAstrology.ExtensionMethods;
using SharpAstrology.HumanDesign.Mathematics;


namespace SharpAstrology.DataModels;

/// <summary>
/// Represents a Human Design Chart. It utilizes planetary positions and activations
/// to provide several complex characteristics like Types, Profiles, Strategies, Channels, Gates and Variables.
/// </summary>
public sealed class HumanDesignChart
{
    /// <summary>
    /// Gets a dictionary of personality activations corresponding to each celestial body. 
    /// </summary>
    public Dictionary<Planets, Activation> PersonalityActivation { get; }
    
    /// <summary>
    /// Gets a dictionary of design activations corresponding to each celestial body. 
    /// </summary>
    public Dictionary<Planets, Activation> DesignActivation { get; }
    
    /// <summary>
    /// Gets a dictionary of connected components, where each center is associated with its components id.
    /// </summary>
    public Dictionary<Centers, int> ConnectedComponents { get; }
    
    /// <summary>
    /// Gets the number of connected components of the Human Design graph.
    /// </summary>
    public int Splits { get; }

    /// <summary>
    /// Initializes a new instance of the <see cref="HumanDesignChart"/> class. It calculates the design date itself.
    /// </summary>
    /// <param name="dateOfBirth">The date of birth for which the Human Design Chart is being created.</param>
    /// <param name="eph">The ephemerides used for planetary positions calculations.</param>
    public HumanDesignChart(DateTime dateOfBirth, IEphemerides eph) : this(dateOfBirth, eph.DesignJulianDay(dateOfBirth), eph) { }
    
    /// <summary>
    /// Initializes a new instance of the <see cref="HumanDesignChart"/> class.
    /// </summary>
    /// <param name="dateOfBirth">The date of birth for which the Human Design Chart is being created.</param>
    /// <param name="designDate">The corresponding design date.</param>
    /// <param name="eph">The ephemerides used for planetary positions calculations.</param>
    public HumanDesignChart(DateTime dateOfBirth, DateTime designDate, IEphemerides eph)
    {
        PersonalityActivation = Definitions.HumanDesignDefaults.HumanDesignPlanets.ToDictionary(
            p => p,
            p => Utility.HumanDesignUtility.ActivationOf(eph.PlanetsPosition(p, dateOfBirth).Longitude));
        DesignActivation = Definitions.HumanDesignDefaults.HumanDesignPlanets.ToDictionary(
            p => p,
            p => Utility.HumanDesignUtility.ActivationOf(eph.PlanetsPosition(p, designDate).Longitude));
        var activeGates = Utility.HumanDesignUtility
            .ActiveGates(PersonalityActivation, DesignActivation);
        (ConnectedComponents, Splits) = GraphService.ConnectedCenters(Utility.HumanDesignUtility.ActiveChannels(activeGates));
    }
    
    private Profiles? _profile;
    /// <summary>
    /// Gets the profile associated with this chart. 
    /// If the value has already been calculated, it retrieves the calculated value. 
    /// If it hasn't been calculated yet, it will call the associated extension method <see cref="HumanDesignChartExtensionMethods.Profile"/>
    /// to calculate its value.
    /// </summary>
    public Profiles Profile
    {
        get
        {
            _profile ??= this.Profile();
            return _profile!.Value;
        }
    }

    private Types? _types;
    /// <summary>
    /// Gets the type associated with this chart.
    /// If the value has already been calculated, it retrieves the calculated value.
    /// If it hasn't been calculated yet, it will call the associated extension method <see cref="HumanDesignChartExtensionMethods.Type"/>
    /// to calculate its value.
    /// </summary>
    public Types Type
    {
        get
        {
            _types ??= this.Type();
            return _types!.Value;
        }
    }

    private Strategies? _strategy;
    /// <summary>
    /// Gets the strategy associated with this chart.
    /// If the value has already been calculated, it retrieves the calculated value.
    /// If it hasn't been calculated yet, it will call the associated extension method <see cref="HumanDesignChartExtensionMethods.Strategy"/>
    /// to calculate its value.
    /// </summary>
    public Strategies Strategy
    {
        get
        {
            _strategy ??= this.Strategy();
            return _strategy!.Value;
        }
    }

    private SplitDefinitions? _splitDefinition;
    /// <summary>
    /// Gets the spilt definition associated with this chart.
    /// If the value has already been calculated, it retrieves the calculated value.
    /// If it hasn't been calculated yet, it will call the associated extension method <see cref="HumanDesignChartExtensionMethods.SplitDefinition"/>
    /// to calculate its value.
    /// </summary>
    public SplitDefinitions SplitDefinition
    {
        get
        {
            _splitDefinition ??= this.SplitDefinition();
            return _splitDefinition!.Value;
        }
    }

    private HashSet<Gates>? _activeGates;
    /// <summary>
    /// Gets the set of active gates of the chart.
    /// If the value has already been calculated, it retrieves the calculated value.
    /// If it hasn't been calculated yet, it will call the associated extension method <see cref="HumanDesignChartExtensionMethods.ActiveGates"/>
    /// to calculate the value.
    /// </summary>
    public HashSet<Gates> ActiveGates
    {
        get
        {
            _activeGates ??= this.ActiveGates();
            return _activeGates;
        }
    }
    
    private HashSet<Channels>? _activeChannels;
    /// <summary>
    /// Gets the set of active channels of the chart.
    /// If the value has already been calculated, it retrieves the calculated value.
    /// If it hasn't been calculated yet, it will call the associated extension method <see cref="HumanDesignChartExtensionMethods.ActiveChannels"/>
    /// to calculate the value.
    /// </summary>
    public HashSet<Channels> ActiveChannels
    {
        get
        {
            _activeChannels ??= this.ActiveChannels();
            return _activeChannels;
        }
    }

    
    private Variables? _variables;
    /// <summary>
    /// Gets the variables associated with this chart.
    /// If the value has already been calculated, it retrieves the calculated value.
    /// If it hasn't been calculated yet, it will call the associated extension method <see cref="HumanDesignChartExtensionMethods.Variables"/>
    /// to calculate its value.
    /// </summary>
    public Variables Variables
    {
        get
        {
            _variables ??= this.Variables();
            return _variables;
        }
    }
}