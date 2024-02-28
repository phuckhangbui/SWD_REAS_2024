import React, { useEffect, useState } from "react";
import news from "../../interface/news";

interface NewsProp {
  news: news;
}

const NewsCard = ({ news }: NewsProp) => {
  const [newsPiece, setNewsPiece] = useState<news | undefined>(news);

  useEffect(() => {
    setNewsPiece(news || undefined);
  }, [news]);

  return (
    <div className="max-w-2xl bg-white border border-gray-200 rounded-lg shadow mx-auto sm:my-2 md:my-0 flex sm:flex-col lg:flex-row">
      <div className="xl:w-60 lg:w-48  sm:w-full flex-shrink-0">
        <img
          className="lg:rounded-tl-lg lg:rounded-bl-lg lg:rounded-tr-none sm:rounded-t-lg sm:h-48 md:h-52 lg:h-48 w-full object-fill"
          src={newsPiece?.thumbnailUri}
          alt=""
        />
      </div>
      <div className="sm:p-5 lg:p-3">
        <h5 className="mb-2 text-2xl font-bold tracking-tight text-gray-900 xl:line-clamp-2 sm:line-clamp-3">
          {newsPiece?.newsTitle}
        </h5>
        <p className="mb-3 font-normal text-gray-900">
          <span className=" text-gray-700">By:</span>{" "}
          <span className="font-bold">{newsPiece?.newsSumary}</span>
        </p>
        {/* <div className="flex justify-between items-center">
          <div className=" tracking-tight text-gray-900 ">
            {formattedDateCreated}
          </div>
        </div> */}
      </div>
    </div>
  );
};

export default NewsCard;
