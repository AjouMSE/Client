using System.Collections;
using System.Threading.Tasks;

namespace Data.Cache
{
    public interface ICacheSource<T>
    {
        public IEnumerator Init();

        public T GetSource(int id);
    }
}