import { ReactNode } from "react";

type TableProps = {
  fields: string[];
  children: ReactNode;
  className?: string;
};
function Table({ fields, children, className=''}: TableProps) {
  return (
    <div className={`block w-full overflow-auto scrolling-touch ${className}`}>
        <table className="w-full max-w-full mb-4">
          <thead className="text-center">
          <tr>
            {fields.map((item, i) => (
              <th key={i} className='text-md px-2 py-2 text-sm uppercase text-slate-600'>{item} </th>
            ))}
          </tr>
        </thead>
        <tbody className="text-gray-600">
          {Array.isArray(children) && children.length == 0?
            <tr><td colSpan={fields.length} className="text-center">Pas de lignes a afficher</td></tr>:
            children
          }
        </tbody>
      </table>
    </div>
  );
}

export default Table;
