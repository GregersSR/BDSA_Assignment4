using System.Collections.Generic;
using Assignment4.Core;
using System.Linq;
using System.Collections.ObjectModel;

namespace Assignment4.Entities
{
    public class TagRepository : ITagRepository
    {
        private readonly KanbanContext _context;
        public TagRepository(KanbanContext context)
        {
            _context = context;
        }

        public (Response Response, int TagId) Create(TagCreateDTO tag)
        {
            var entity = new Tag { Name = tag.Name };
            
            _context.Tags.Add(entity);
            _context.SaveChanges();
            return (Response.Created, entity.Id);
        }

        public Response Delete(int tagId, bool force = false)
        {
            var entity = _context.Tags.Find(tagId);
            if (entity == null) {
                return Response.NotFound;
            } else {
                _context.Tags.Remove(entity);
                _context.SaveChanges();
                return Response.Deleted;
            }
        }

        public TagDTO Read(int tagId)
        {
            var entity = _context.Tags.Find(tagId);
            return entity == null ? null : new TagDTO(entity.Id, entity.Name);
        }

        public IReadOnlyCollection<TagDTO> ReadAll()
        {
            var query = from tag in _context.Tags
                        select new TagDTO(tag.Id, tag.Name);
            return new ReadOnlyCollection<TagDTO>(query.ToList());
        }

        public Response Update(TagUpdateDTO tag)
        {
            var entity = _context.Tags.Find(tag.Id);
            if (entity == null) {
                return Response.NotFound;
            }

            entity.Name = tag.Name;
            _context.SaveChanges();

            return Response.Updated;
        }
    }
}
