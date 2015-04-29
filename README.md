#FastMapper

Powerful convention-based, customizable and fastest tool for object-object mapping.It is useful for Entity to DTO, DTO to Entity mapping strategies.

###Where can I get it?

The latest builds can be found at NuGet

    PM> Install-Package FastMapper

###Getting Started

####Mapping to a new object

FastMapper makes the object and maps values to it.

    TDestination destObject = TypeAdapter.Adapt<TSource, TDestination>(sourceObject);

####Mapping to an existing object

You make the object, FastMapper maps to the object.

    TDestination destObject = new TDestination();
    destObject = TypeAdapter.Adapt(sourceObject, destObject);

####Mapping Lists Included

This includes lists, arrays, collections, enumerables etc...

    var destObjectList = TypeAdapter.Adapt<List<TSource>, List<TDestination>>(sourceList);

####Customized Mapping

When the default convention mappings aren't enough to do the job, you can specify complex source mappings.

    TypeAdapterConfig<TSource, TDestination>()
    .NewConfig()
    .IgnoreMember(dest => dest.Property)
    .Map(dest => dest.FullName, 
             src => string.Format("{0} {1}", src.FirstName, src.LastName));


####Queryable Extensions

    using(MyDbContext context = new MyDbContext())
    {
        // Build a Select Expression from DTO
        var destinations = context.Sources.Project().To<Destination>().ToList();
    
        // Versus creating by hand:
        var destinations = context.Sources.Select(c => new Destination(){
            Id = p.Id,
            Name = p.Name,
            Surname = p.Surname,
            ....
        })
        .ToList();
    }
    
####Max Depth

When mapping nested or tree-type structures, it's often necessary to specify a max nesting depth to prevent overflows.

    TypeAdapterConfig<TSource, TDestination>
                .NewConfig()
                .MaxDepth(3);

###Performance Comparisons

When aggregating times across complex and simple objects, we're seeing a ~30x speed improvement in comparison to AutoMapper, but it's obviously very dependent upon your situation.

####Benchmark "Complex" Object

The following test converts a Customer object with 2 nested address collections and two nested address sub-objects to a DTO.

    Customer c = new Customer()
    {
        Address = new Address() { City = "istanbul", Country = "turkey", Id = 1, Street = "istiklal cad." },
        HomeAddress = new Address() { City = "istanbul", Country = "turkey", Id = 2, Street = "istiklal cad." },
        Id = 1,
        Name = "John Doe",
        Credit = 234.7m,
        WorkAddresses = new List<Address>() { 
            new Address() { City = "istanbul", Country = "turkey", Id = 5, Street = "istiklal cad." },
            new Address() { City = "izmir", Country = "turkey", Id = 6, Street = "konak" }
        },
        Addresses = new List<Address>() { 
            new Address() { City = "istanbul", Country = "turkey", Id = 3, Street = "istiklal cad." },
            new Address() { City = "izmir", Country = "turkey", Id = 4, Street = "konak" }
        }.ToArray()
    };

Competitors : Handwriting Mapper, FastMapper, AutoMapper

    Iterations : 100
    
    Handwriting Mapper : 2 miliseconds
    FastMapper : 0 miliseconds
    AutoMapper : 82 miliseconds
    
    Iterations : 1000
    
    Handwriting Mapper : 2 miliseconds
    FastMapper : 2 miliseconds
    AutoMapper : 106 miliseconds
    
    Iterations : 10000
    
    Handwriting Mapper : 6 miliseconds
    FastMapper : 31 miliseconds
    AutoMapper : 1088 miliseconds
    
    Iterations : 100000
    
    Handwriting Mapper : 62 miliseconds
    FastMapper : 289 miliseconds
    AutoMapper : 10620 miliseconds
    
    Iterations : 1000000
    
    Handwriting Mapper : 608 miliseconds
    FastMapper : 2976 miliseconds
    AutoMapper : 106336 miliseconds
