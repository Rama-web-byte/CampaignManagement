import React from "react";

const PageHeader = ({ title, subtitle }) => {
  return (
    <div className="text-center py-8">
      <h1 className="text-4xl md:text-5xl font-bold text-white">{title}</h1>
      {subtitle && <p className="text-gray-200 mt-2">{subtitle}</p>}
    </div>
  );
};

export default PageHeader;
