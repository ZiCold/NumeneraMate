using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NumeneraMate.Libs.DataAccess
{
	interface IRepo<T>
	{
		int Add(T entity);
		int AddRange(IList<T> entities);
		int Save(T entity);
		int Delete(T entity);

		T GetOne(string name);
		List<T> GetAll();
	}
}
