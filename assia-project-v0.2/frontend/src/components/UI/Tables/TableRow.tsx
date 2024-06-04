import { ReactNode } from "react";

type TableRowProps = {
  children: ReactNode;
};

function TableRow({ children }: TableRowProps) {
  return (<tr>
    {children}
    </tr>);
}

export default TableRow;
