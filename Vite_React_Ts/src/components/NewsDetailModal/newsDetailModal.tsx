import { useState } from "react";

interface NewsDetailModalProps {
  newsId: number;
  closeModal: () => void;
}

const NewsDetailModal = ({ closeModal, newsId }: NewsDetailModalProps) => {
  // const [newsDetail, setNewsDetail] = useState<news>()
  return (
    <div className="relative w-full max-w-7xl max-h-full ">
      <div className="relative bg-white rounded-lg shadow md:px-10 md:pb-5 sm:px-0 sm:pb-0 ">
        <div className=" items-center justify-start md:py-5 md:px-0 sm:p-5 z-10 top-0">
          <button
            type="button"
            className="justify-start bg-transparent md:bg-transparent sm:bg-white sm:bg-opacity-60 rounded-3xl text-sm w-10 h-10 ms-auto inline-flex items-center "
            data-modal-hide="default-modal"
            onClick={closeModal}
          >
            <svg
              className="w-6 h-6 sm:text-black sm:hover:text-mainBlue "
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

          <div className="">
            <div className="text-4xl p-4 font-bold">
              People should stop expecting real estate to become cheaper
            </div>
            <div>
              <span className="text-gray-900">By: </span>
              <span className="font-bold">John Doe</span>
            </div>
            <div className="text-gray-500 italic">2/1/2024</div>
          </div>

          <div className="flex items-center justify-center w-full mx-auto py-2 max-w-3xl ">
            <img
              className="object-contain"
              src="https://i1-english.vnecdn.net/2024/02/13/dsc01956jpg1703645102-17072961-3107-8315-1707786539.jpg?w=680&h=408&q=100&dpr=1&fit=crop&s=aFCqlC582P4eydtXC-U8Ag"
              alt=""
            />
          </div>

          <div className="">
            <div>
              <p className="text-base leading-relaxed text-gray-900">
                Older generations had to wait decades for their properties to
                rise to the current prices, so younger generations should do the
                same instead of complaining about expensive real estate. I
                reside in a residential area in the Binh Duong Province, which
                is adjacent to Ho Chi Minh City’s Thu Duc City. It is a small
                neighborhood comprising several dozen houses where one can
                encounter dialects from all three regions of the nation. Migrant
                workers came to this area years ago to make a name for
                themselves, all the while living in cramped boarding houses.
                Now, everyone here has their own houses thanks to decades of
                hard work and saving, and a bit of luck. For instance, a couple
                selling groceries in my neighborhood was fortunate enough to
                have a house situated on a major road. They mentioned that about
                15-20 years ago, the neighborhood was a poor area with no street
                lights and a road that was only wide enough for a motorcycle to
                pass through. At that time, they saved and borrowed an
                additional VND250 million (US$10,235) to purchase the plot of
                land. They only received a hand-written note confirming the
                transaction and were only able to secure an official land title
                much later. Now, they have a well-constructed and beautiful
                home. Last year, someone offered VND5 billion to buy the house
                for business purposes and the couple are considering selling it
                to move to another house on a smaller alley. I came here much
                later when the roads had already been developed and widened. By
                then the land price here had risen, but it was still affordable.
                I remember when I first moved here, the streets were so deserted
                that I always tried to return home before 9 p.m. due to fears of
                robbery. A few years later, many families came here to build
                houses, coffee shops and restaurants, making the area more
                bustling. Many people hope that house prices will decrease when
                the revised Land Law takes effect. I, however, do not share this
                view. The new law might stabilize real estate prices and curb
                speculation, but it definitely cannot reduce house prices as
                many people anticipate. And even when prices have stabilized,
                many people will compete to purchase homes and land, so
                properties near city centers will not be any easier to buy.
                Therefore, like the families in my neighborhood, if you cannot
                buy a house, you should purchase land in undeveloped and
                secluded areas like the city's outskirts and then wait for it to
                develop. Previous generations did so and waited for 15, 20 years
                or even half of their lives. So the current generation not being
                able to buy a house at a young age is not necessarily an unjust
                thing. So what is your viewpoint on this matter?
              </p>
            </div>
          </div>
        </div>
        <footer>
          <div className="w-full max-w-screen-xl">
            <hr className="mb-6 border-gray-200 sm:mx-auto lg:mb-8 " />
            <span className="block text-sm text-gray-900 sm:text-center xs:text-center sm:pb-8 md:pb-0">
              © 2023 REAS™ . All Rights Reserved.
            </span>
          </div>
        </footer>
      </div>
    </div>
  );
};

export default NewsDetailModal;
