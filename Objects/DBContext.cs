
using MyORM.Attributes;
using MyORM.Interfaces;
using MyORM.Exceptions;

using System.Data;
using System.Reflection;


namespace MyORM.Objects
{
    /// <summary>
    /// Default implementation that determines how the database must be updated
    /// </summary>
    public abstract class DBContext : IDBContext
    {
        /*
         * Esta classe contem uma definição geral para criação e atualização dos
         * bancos e tabelas. 
         * Nessa classe tambem temos metodos para obter coleções de tipos mapeados a partir do Type          
         */

        //dependecy injection of a IDBManager
        IDBManager _dBManager;

        //the list of types mapped in this context
        private IList<Type>? _mappedTypes;

        /// <summary>
        /// The list of types mapped in this context
        /// </summary>
        public IEnumerable<Type> MappedTypes 
        {
            get
            {
                if(_mappedTypes != null)
                    return _mappedTypes;

                /*
                 * obtem todas as propriedades que são do tipo IEntityCollection, ou seja
                 * todos os "DBSet" do context
                 */

                IEnumerable<PropertyInfo> propertyInfos = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(d => d.PropertyType.IsAssignableTo(typeof(IEntityCollection)));  

                 _mappedTypes = new List<Type>();

                foreach (PropertyInfo property in propertyInfos)
                {
                    Type[] typesgenerics = property.PropertyType.GetGenericArguments();

                    if(typesgenerics.Length > 0)
                        _mappedTypes.Add(typesgenerics[0]);

                }

                return _mappedTypes;
            }

        }

        /// <summary>
        /// default ctor
        /// </summary>
        /// <param name="dBManager">The Object that represents a database instance</param>
        public DBContext(IDBManager dBManager)
        {
            _dBManager = dBManager;

        }


        /// <summary>
        /// Return a IEntityCollection from generic type passed as generic argument
        /// </summary>
        /// <typeparam name="T">The type of object that was mapped</typeparam>
        /// <returns>Return a IEntityCollection of the type passed as param</returns>
        /// <exception cref="NoEntityMappedException">Throw a NoEntityMappedException when the type passed as param was not mapped</exception>
        public IEntityCollection<T>? Collection<T>() where T : class
        {
            //obtem todas as propriedades IEntityCollection 
            IEnumerable<PropertyInfo> propertyInfos = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(d => d.PropertyType.IsAssignableTo(typeof(IEntityCollection)));


            foreach (PropertyInfo property in propertyInfos)
            {
                Type?[] typesgenerics = property.PropertyType.GetGenericArguments(); //obtem os tipos genericos dessas propriedades

                if (typesgenerics[0] == typeof(T))// se o tipo generico for igual ao passado como parametro
                {
                    return property.GetValue(this) as IEntityCollection<T>; //retorna a propriedade castiada como IEntityCollection<T>
                }

            }

            //se não achou nenhuma propriedade, lança uma exection
            throw new NoEntityMappedException($"No one IEntityCollection was found to the type {typeof(T).Name}");

        }


        /// <summary>
        /// Return a IEntityCollection from type passed 
        /// </summary>
        /// <param name="collectionType">The type of object that as mapped</param>
        /// <returns>Return a IEntityCollection of the type passed as param</returns>
        /// <exception cref="NoEntityMappedException">Throw a NoEntityMappedException when the type passed as param was not mapped</exception>
        public IEntityCollection? Collection(Type collectionType)
        {
            IEnumerable<PropertyInfo> propertyInfos = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance)
                .Where(d => d.PropertyType.IsAssignableTo(typeof(IEntityCollection))); //obtem todas as propriedades do tipo IEntityCollection


            foreach (PropertyInfo property in propertyInfos)
            {
                Type?[] typesgenerics = property.PropertyType.GetGenericArguments(); //obtem os tipos genericos dessa propriedade

                if (typesgenerics[0] == collectionType) // se o tipo generico da propriedade for igual ao tipo de objeto passado
                {
                    return property.GetValue(this) as IEntityCollection; //retorna essa propriedade
                }

            }

            //se não achou nenhuma propriedade, lança uma exection
            throw new NoEntityMappedException($"No one IEntityCollection was found to the type {collectionType.Name}");

        }


        /// <summary>
        /// Test the connection  with database
        /// </summary>
        /// <returns>Return a boolean that represent the result of test</returns>
        public bool TestConnection()
        {
            return _dBManager.TryConnection();
        }

        /// <summary>
        /// Create the database
        /// </summary>
        public void CreateDataBase()
        {
            _dBManager.CreateDataBase();
        }

        /// <summary>
        /// Drop the database
        /// </summary>
        public void DropDataBase()
        {
            _dBManager.DropDataBase();
        }

        /// <summary>
        /// Update the database, that is, will create the database and tables
        /// </summary>
        /// <exception cref="NoEntityMappedException">Will thow a NoEntityMappedException if no one type as mapped</exception>
        public void UpdateDataBase()
        {
            if (!_dBManager.DataBaseExists()) //verifica se o banco ja existe
            {
                _dBManager.CreateDataBase(); //se não existir, cria
            }

            //pega todas as propriedade que podem ser atribuidas a uma variavel do tipo IEntityCollection
            IEnumerable<PropertyInfo> propertyInfos = this.GetType().GetProperties(BindingFlags.Public | BindingFlags.Instance).Where(d => d.PropertyType.IsAssignableTo(typeof(IEntityCollection)));

            //se não tem nenhuma propriedade dessas, throw uma execption 
            if (!propertyInfos.Any())
            {
                throw new NoEntityMappedException("No one entity was mapped");
            }

            //para cada IEntityCollection 
            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                //obtem o nome da tabela
                string tableName = 
                    propertyInfo.PropertyType.GetCustomAttribute<DBTableAttribute>()?.Name ?? //nome do atributo
                    propertyInfo.PropertyType.GenericTypeArguments[0].Name.ToLower(); // ou nome do tipo


                MethodInfo? method = _dBManager.GetType().GetMethod("CreateTable"); // obtem as especficações do metodo para criar a tabela

                /*
                 * obtem as especificações para o metodo de criar tabelas generico
                 */
                MethodInfo? genericMethod = method?.MakeGenericMethod(propertyInfo.PropertyType.GenericTypeArguments[0]); 
                
                genericMethod?.Invoke(_dBManager, null); //chama o metodo passando o objeto que contem o metodo e os argumentos

                Type? propertyType = propertyInfo?.PropertyType?.GenericTypeArguments[0]; //obtem o tipo generico da proprieda, ou seja o tipo mapeado

                if (propertyType is null)
                    continue;

                /*
                 * obtem as propriedades do tipo mapeado para criação da tabela
                 */
                IEnumerable<PropertyInfo> typePropertyInfos = propertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(d => d.GetCustomAttribute<DBIgnoreAttribute>() == null) //que não possua a tag ignore
                    .Where(d => d.GetCustomAttribute<DBForeignKeyAttribute>() == null); //que não seja foreign key, pois, elas são criadas apos as tabelas

                foreach (PropertyInfo property in typePropertyInfos)
                {
                    _dBManager.CreateColumn(tableName, property); //cria a coluna
                }

            }

            foreach (PropertyInfo propertyInfo in propertyInfos)
            {
                //obtem o nome da tabela
                string tableName =
                    propertyInfo.PropertyType.GetCustomAttribute<DBTableAttribute>()?.Name ?? //nome do atributo
                    propertyInfo.PropertyType.GenericTypeArguments[0].Name.ToLower(); // ou nome do tipo

                Type? propertyType = propertyInfo?.PropertyType?.GenericTypeArguments[0]; //obtem o tipo do objecto mappeado

                if (propertyType is null)
                    continue;

                /*
                 * obtem as propriedades do tipo mapeado que sejam foreign key para criação das constraints
                 */
                IEnumerable<PropertyInfo> typePropertyInfos = propertyType.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                    .Where(d => d.GetCustomAttribute<DBIgnoreAttribute>() == null);

                foreach (PropertyInfo property in typePropertyInfos.Where(d => d.GetCustomAttribute<DBForeignKeyAttribute>() != null))
                {
                    _dBManager.CreateColumn(tableName, property); //cria as colunas
                }

                //apos a criação das colunas, ajustamos a tabela para ter apenas as colunas do objeto atual (caso downgrade)
                _dBManager.FitColumns(tableName, typePropertyInfos);
            }


        }


    }
}
