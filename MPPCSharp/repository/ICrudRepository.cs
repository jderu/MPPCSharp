using System.Collections.Generic;
using log4net;
using MPPCSharp.domain;

namespace MPPCSharp.repository {
    public interface ICrudRepository<TId, TE> where TE : Entity<TId> {
       private static readonly ILog _log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

       public static ILog Log => _log;

       /**
     * @param id -the id of the entity to be returned
     *           id must not be null
     * @return the entity with the specified id
     * or null - if there is no entity with the given id
     * @throws IllegalArgumentException if id is null.
     */
        public TE FindOne(TId id);

        /**
     * @return all entities
     */
        public List<TE> FindAll();

        /**
     * @param entity entity must be not null
     * @return null- if the given entity is saved
     * otherwise returns the entity (id already exists)
     * @throws ValidationException      if the entity is not valid
     * @throws IllegalArgumentException if the given entity is null. *
     */
        public TE Save(TE entity);

        /**
     * removes the entity with the specified id
     *
     * @param id id must be not null
     * @return the removed entity or null if there is no entity with the
     * given id
     * @throws IllegalArgumentException if the given id is null.
     */
        public TE Delete(TId id);

        /**
     * @param entity entity must not be null
     * @return null - if the entity is updated,
     * otherwise returns the entity - (e.g id does not
     * exist).
     * @throws IllegalArgumentException if the given entity is null.
     * @throws ValidationException      if the entity is not valid.
     */
        public TE Update(TE entity);
    }
}