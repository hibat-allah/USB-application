import { ReactNode } from "react";

type TableCellProps = {
  children: ReactNode;
  className?: string;
};

function TableCell({ children, className }: TableCellProps) {
  return <td className={`py-2 px-2 text-center ${className}`}>{children}</td>;
}

export default TableCell;
