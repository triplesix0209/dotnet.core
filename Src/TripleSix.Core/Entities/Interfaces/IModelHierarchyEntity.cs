using System;
using System.Collections.Generic;

namespace TripleSix.Core.Entities
{
    public interface IModelHierarchyEntity<TEntity> : IModelEntity
        where TEntity : IModelEntity
    {
        Guid? HierarchyParentId { get; set; }

        TEntity HierarchyParent { get; set; }

        IList<TEntity> HierarchyChilds { get; set; }
    }
}