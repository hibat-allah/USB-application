import { ReactNode } from "react";

type ColorsProps = {
  bgColor: string;
  textColor: string;
  children: ReactNode;
  className?: string;
  title?: string;
};

function Badge({ bgColor, textColor, children, className='', title='' }: ColorsProps) {
  return (
    <span className={`py-1.5 px-3.6 text-xs rounded-3xl inline-block whitespace-nowrap text-center font-bold leading-none ${className}`} title={title} style={{ backgroundColor: bgColor, color: textColor }}>
      {children}
    </span>
  );
}

export default Badge;
