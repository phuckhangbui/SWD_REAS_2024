import RealEstateList from "../../../components/RealEstate/realEstateList";
import realEstate from "../../../interface/realEstate";
import { realEstatesList } from "../../../data/realEstate.ts";
import SearchBar from "../../../components/SearchBar/searchBar.tsx";
import NewsList from "../../../components/News/newsList.tsx";
import news from "../../../interface/news.ts";
import { newsList } from "../../../data/news.ts";

const HomePage = () => {
  const realEstates: realEstate[] = realEstatesList;
  const news: news[] = newsList;
  return (
    <div>
      <div className="pt-20">
        <SearchBar />
      </div>
      <div className="pt-8">
        <div className="container w-full mx-auto">
          <div className="text-center">
            <div className="text-gray-900  text-4xl font-bold">
              What's New Today
            </div>
            <div className="mt-2">
              See the latest news about the current real estates market
            </div>
          </div>

          <NewsList newsList={news} />
        </div>
      </div>
      <div className="pt-8">
        <div className="container w-full mx-auto">
          <div className="text-center">
            <div className="text-gray-900  text-4xl font-bold">
              Explore Our Real Estate Options
            </div>
            <div className="mt-2">
              Take a look at our various options and find your forever home
            </div>
          </div>
          <RealEstateList realEstatesList={realEstates} />
        </div>
      </div>
      <div className="pt-8">
        <div className="container w-full mx-auto">
          <div className="text-center">
            <div className="text-gray-900  text-4xl font-bold">
              Take Part in Our Most Popular Auctions
            </div>
            <div className="mt-2">
              Participate and try your best to win your dream home
            </div>
          </div>
          <RealEstateList realEstatesList={realEstates} />
        </div>
      </div>
    </div>
  );
};

export default HomePage;
