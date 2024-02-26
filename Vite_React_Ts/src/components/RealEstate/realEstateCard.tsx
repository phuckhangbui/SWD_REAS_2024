import React, { useEffect, useState } from "react";
import realEstate from "../../interface/realEstate";
import { Link } from "react-router-dom";
interface RealEstateProps {
  realEstate: realEstate;
}

const RealEstateCard = ({ realEstate }: RealEstateProps) => {
  const [estate, setEstate] = useState<realEstate | undefined>(realEstate);
  const [formattedDateEnd, setFormattedDateEnd] = useState<string>("");
  const options: Intl.DateTimeFormatOptions = {
    day: "2-digit",
    month: "2-digit",
    year: "numeric"
  }

  useEffect(() => {
    setEstate(realEstate || undefined);
    if (realEstate?.dateEnd) {
      const dateObject = new Date(realEstate.dateEnd);
      const formattedDate = dateObject.toLocaleDateString("en-GB", options).replace(/\//g, '-');
      setFormattedDateEnd(formattedDate);
    } 
  }, []);
  // useEffect(() => {
  //   console.log(realEstate);
  // },[realEstate])
  return (
    <div className="max-w-sm bg-white border border-gray-200 rounded-lg shadow mx-auto sm:my-2 md:my-0">
      <div className="">
        <img className="rounded-t-lg h-52 w-full" src={estate?.uriPhotoFirst} alt="" />
      </div>
      <div className="p-5">
        <div>
          <h5 className="mb-2 text-2xl font-bold tracking-tight text-gray-900 line-clamp-3 ">
            {estate?.reasName}
          </h5>
        </div>
        <p className="mb-3 font-normal text-gray-700">
          {estate?.reasTypeName}
        </p>
        <p className="mb-3 font-normal text-gray-700 truncate">
          {/* {realEstate.address} */}
        </p>
        <div className="flex justify-between items-center">
          <div className="text-xl font-bold tracking-tight text-gray-900 ">
            ${estate?.reasPrice}
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
