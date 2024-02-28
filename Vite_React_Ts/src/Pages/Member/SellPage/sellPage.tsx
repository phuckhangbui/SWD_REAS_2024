import { SubmitHandler, useForm } from "react-hook-form";



const SellPage = () => {
  const {
    register,
    handleSubmit,
    formState: { errors },
  } = useForm<createRealEstate>();
  const onSubmit: SubmitHandler<createRealEstate> = (data) => {
    console.log(data);
  };

  return (
    <div className="pt-20">
      <div className="max-w-screen-xl flex flex-wrap items-center justify-between mx-auto p-4">
        <div className="bg-white border border-gray-200 rounded-lg shadow mx-auto w-full px-10 py-5">
          <div className="text-center">
            <div className="text-gray-900  text-4xl font-bold">
              Real Estate Request
            </div>
            <div className="mt-2">
              Send us your request to auction your house so we can help you can
              sell your estate at a profitable price
            </div>
          </div>
          <form className="" onSubmit={handleSubmit(onSubmit)}>
            <div className="grid grid-cols-4 gap-2 p-5">
              <div className="col-span-3">
                <label
                  htmlFor="reasName"
                  className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >
                  Title
                </label>
                <input
                  type="text"
                  id="reasName"
                  className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5  "
                  placeholder="Title"
                  required
                />
              </div>
              <div className="col-span-1">
                <label
                  htmlFor="reasArea"
                  className="block mb-2 text-sm font-medium text-gray-900 dark:text-white"
                >
                  Area
                </label>
                <input
                  type="text"
                  id="reasArea"
                  className="bg-gray-50 border border-gray-300 text-gray-900 text-sm rounded-lg focus:ring-blue-500 focus:border-blue-500 block w-full p-2.5 "
                  placeholder="Area (Sqrt)"
                  required
                />
              </div>
            </div>
          </form>
        </div>
      </div>
    </div>
  );
};

export default SellPage;
