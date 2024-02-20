import React from "react";
import realEstate from "../../interface/realEstate";
import { Link } from "react-router-dom";
interface RealEstateProps {
  realEstate: realEstate;
}

const RealEstateCard = ({ realEstate }: RealEstateProps) => {
  const formattedDateEnd = realEstate.dateEnd.toLocaleDateString();
  return (
    <div className="max-w-sm bg-white border border-gray-200 rounded-lg shadow mx-auto sm:my-2 md:my-0">
      <div>
        <img className="rounded-t-lg" src={realEstate.image} alt="" />
      </div>
      <div className="p-5">
        <div>
          <h5 className="mb-2 text-2xl font-bold tracking-tight text-gray-900 truncate ">
            {realEstate.name}
          </h5>
        </div>
        <p className="mb-3 font-normal text-gray-700 line-clamp-2">
          {realEstate.description}
        </p>
        <p className="mb-3 font-normal text-gray-700 truncate">
          {realEstate.address}
        </p>
        <div className="flex justify-between items-center">
          <div className="text-xl font-bold tracking-tight text-gray-900 ">
            ${realEstate.price}
          </div>
          <div className=" tracking-tight text-gray-900 ">
            Due: {formattedDateEnd}
          </div>
        </div>
      </div>
    </div>
  );
};

export default RealEstateCard;
