import { Carousel } from "@material-tailwind/react";
import { useEffect, useRef, useState } from "react";
import { getRealEstateById } from "../../api/realEstate";

interface RealEstateDetailModalProps {
  realEstateId: number;
  closeModal: () => void;
  address: string;
  index: string;
}

const RealEstateDetailModal = ({
  closeModal,
  realEstateId,
  index,
}: RealEstateDetailModalProps) => {
  const [tabStatus, setTabStatus] = useState(index);
  const [realEstateDetail, setRealEstateDetail] = useState<
    realEstateDetail | undefined
  >();
  const [formattedDateEnd, setFormattedDateEnd] = useState<string>("");

  useEffect(() => {
    try {
      const fetchRealEstateDetail = async () => {
        const response = await getRealEstateById(realEstateId);
        setRealEstateDetail(response);
      };
      fetchRealEstateDetail();
    } catch (error) {
      console.log(error);
    }
  }, []);

  useEffect(() => {
    try {
      if (realEstateDetail?.dateEnd) {
        const dateObject = new Date(realEstateDetail.dateEnd);
        const formattedDate = dateObject
          .toDateString()
          .split(" ")
          .slice(1)
          .join(" ");
        setFormattedDateEnd(formattedDate);
        console.log(formattedDate);
      }
    } catch (error) {
      console.log(error);
    }
  }, [realEstateDetail?.dateEnd]);

  // Change the tab index
  const toggleTab = (index: string) => {
    setTabStatus(index);
  };

  // Tab button status
  const getActiveTab = (index: string) => {
    return `${
      index === tabStatus
        ? "text-mainBlue border-mainBlue font-bold"
        : "border-transparent hover:border-gray-900"
    } text-xl  border-b-2 rounded-t-lg`;
  };

  // Changing the tab description
  const getActiveTabDetail = (index: string) => {
    return `${index === tabStatus ? "" : "hidden"} mt-2 space-y-4 `;
  };

  return (
    <div className="relative w-full max-w-7xl max-h-full ">
      <div className="relative bg-white rounded-lg shadow md:px-10 md:pb-5 sm:px-0 sm:pb-0 ">
        <div className=" items-center justify-start md:py-5 md:px-0 sm:p-5 sm:fixed md:static z-10 top-0">
          <button
            type="button"
            className=" bg-transparent md:bg-transparent sm:bg-white sm:bg-opacity-60 rounded-3xl text-sm w-10 h-10 ms-auto inline-flex justify-center items-center "
            data-modal-hide="default-modal"
            onClick={closeModal}
          >
            <svg
              className="w-6 h-6  sm:text-black sm:hover:text-mainBlue "
              aria-hidden="true"
              xmlns="http://www.w3.org/2000/svg"
              fill="none"
              viewBox="0 0 14 10"
            >
              <path
                stroke="currentColor"
                strokeLinecap="round"
                strokeLinejoin="round"
                strokeWidth="2"
                d="M13 5H1m0 0 4 4M1 5l4-4"
              />
            </svg>
            <span className="sr-only">Close modal</span>
          </button>
        </div>

        <div className="md:mx-24 ">
          <div className="top-5">
            <Carousel className=" rounded-lg">
              {realEstateDetail?.photos?.map((photo) => (
                <img
                  src={photo.reasPhotoUrl}
                  alt="Real Estate Photos"
                  className="md:h-120 sm:h-96 w-full object-fill rounded-lg"
                  key={photo.reasPhotoId}
                />
              ))}
            </Carousel>
          </div>
        </div>
        <hr className="mt-8 mb-6 border-gray-200 sm:mx-auto " />
        <div className=" md:mb-0 sm:px-4 lg:px-30">
          <div className="">
            <div className="text-4xl font-bold text-justify">
              {realEstateDetail?.reasName}
            </div>
            <div>
              <ul className="mt-2 flex flex-row gap-4">
                <li>
                  <button
                    className={getActiveTab("detail")}
                    onClick={() => toggleTab("detail")}
                  >
                    Detail
                  </button>
                </li>
                <li>
                  <button
                    className={getActiveTab("auction")}
                    onClick={() => toggleTab("auction")}
                  >
                    Auction
                  </button>
                </li>
              </ul>
            </div>
          </div>
          <div className={getActiveTabDetail("detail")}>
            <div className="grid md:grid-cols-4 sm:grid-cols-2 gap-2">
              <div className=" md:col-span-2 flex items-center">
                <div>
                  <svg
                    className="w-6 h-6 mr-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke="currentColor"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M8 17.3a5 5 0 0 0 2.6 1.7c2.2.6 4.5-.5 5-2.3.4-2-1.3-4-3.6-4.5-2.3-.6-4-2.7-3.5-4.5.5-1.9 2.7-3 5-2.3 1 .2 1.8.8 2.5 1.6m-3.9 12v2m0-18v2.2"
                    />
                  </svg>
                </div>
                <div>
                  <div className="text-xl font-bold ">
                    {realEstateDetail?.reasPrice},000 VND
                  </div>
                  <div className="text-xs text-gray-700">Starting Price</div>
                </div>
              </div>
              <div className=" flex items-center ">
                <div className="">
                  <svg
                    className="w-6 h-6 mr-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke="currentColor"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M9 7H7m2 3H7m2 3H7m4 2v2m3-2v2m3-2v2M4 5v14c0 .6.4 1 1 1h14c.6 0 1-.4 1-1v-3c0-.6-.4-1-1-1h-9a1 1 0 0 1-1-1V5c0-.6-.4-1-1-1H5a1 1 0 0 0-1 1Z"
                    />
                  </svg>
                </div>
                <div>
                  <div className="text-xl font-bold ">
                    {realEstateDetail?.reasArea}
                  </div>
                  <div className="text-xs text-gray-700">Sqft</div>
                </div>
              </div>
              <div className="flex items-center">
                <div>
                  <svg
                    className="w-6 h-6 mr-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke="currentColor"
                      stroke-width="2"
                      d="M7 17v1c0 .6.4 1 1 1h8c.6 0 1-.4 1-1v-1a3 3 0 0 0-3-3h-4a3 3 0 0 0-3 3Zm8-9a3 3 0 1 1-6 0 3 3 0 0 1 6 0Z"
                    />
                  </svg>
                </div>
                <div>
                  <div className="text-xl font-bold ">
                    {realEstateDetail?.accountOwnerName}
                  </div>
                  <div className="text-xs text-gray-700">Uploaded by</div>
                </div>
              </div>
              <div className="md:col-span-2  flex items-center">
                <div>
                  <svg
                    className="w-6 h-6 mr-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke="currentColor"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M6 4h12M6 4v16M6 4H5m13 0v16m0-16h1m-1 16H6m12 0h1M6 20H5M9 7h1v1H9V7Zm5 0h1v1h-1V7Zm-5 4h1v1H9v-1Zm5 0h1v1h-1v-1Zm-3 4h2a1 1 0 0 1 1 1v4h-4v-4a1 1 0 0 1 1-1Z"
                    />
                  </svg>
                </div>
                <div>
                  <div className="text-xl font-bold ">
                    {realEstateDetail?.reasAddress}
                  </div>
                  <div className="text-xs text-gray-700">Property address</div>
                </div>
              </div>
              <div className=" flex items-center">
                <div>
                  <svg
                    className="w-6 h-6 mr-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke="currentColor"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="m4 12 8-8 8 8M6 10.5V19c0 .6.4 1 1 1h3v-3c0-.6.4-1 1-1h2c.6 0 1 .4 1 1v3h3c.6 0 1-.4 1-1v-8.5"
                    />
                  </svg>
                </div>
                <div>
                  <div className="text-xl font-bold ">
                    {realEstateDetail?.type_REAS_Name}
                  </div>
                  <div className="text-xs text-gray-700">Property type</div>
                </div>
              </div>
              <div className=" flex items-center">
                <div>
                  <svg
                    className="w-6 h-6 mr-2"
                    aria-hidden="true"
                    xmlns="http://www.w3.org/2000/svg"
                    fill="none"
                    viewBox="0 0 24 24"
                  >
                    <path
                      stroke="currentColor"
                      stroke-linecap="round"
                      stroke-linejoin="round"
                      stroke-width="2"
                      d="M4 10h16m-8-3V4M7 7V4m10 3V4M5 20h14c.6 0 1-.4 1-1V7c0-.6-.4-1-1-1H5a1 1 0 0 0-1 1v12c0 .6.4 1 1 1Zm3-7h0v0h0v0Zm4 0h0v0h0v0Zm4 0h0v0h0v0Zm-8 4h0v0h0v0Zm4 0h0v0h0v0Zm4 0h0v0h0v0Z"
                    />
                  </svg>
                </div>
                <div>
                  <div className="text-xl font-bold ">{formattedDateEnd}</div>
                  <div className="text-xs text-gray-700">Due date</div>
                </div>
              </div>
            </div>
            <div className="">
              <div className="text-xl font-bold ">Description</div>
              <div
                dangerouslySetInnerHTML={{
                  __html: realEstateDetail?.reasDescription || "",
                }} className="mt-1"
              ></div>
            </div>
          </div>
          <div className={getActiveTabDetail("auction")}>
            <p className="text-base leading-relaxed text-gray-900">
              With less than a month to go before the European Union enacts new
              consumer privacy laws for its citizens, companies around the world
              are updating their terms of service agreements to comply.
            </p>
            <p className="text-base leading-relaxed text-gray-900">
              With less than a month to go before the European Union enacts new
              consumer privacy laws for its citizens, companies around the world
              are updating their terms of service agreements to comply.
            </p>
            <p className="text-base leading-relaxed text-gray-900">
              With less than a month to go before the European Union enacts new
              consumer privacy laws for its citizens, companies around the world
              are updating their terms of service agreements to comply.
            </p>
          </div>
        </div>
        <hr className="my-6 border-gray-200 sm:mx-auto lg:my-8 " />
        <footer>
          <div className="w-full max-w-screen-xl">
            <span className="block text-sm text-gray-900 sm:text-center xs:text-center sm:pb-8 md:pb-0">
              © 2023 REAS™ . All Rights Reserved.
            </span>
          </div>
        </footer>
      </div>
    </div>
  );
};

export default RealEstateDetailModal;
