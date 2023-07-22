export const CarCardSkeleton = () => {
  return (
    <div className="p-4 relative max-h-[470px] lg:max-h-[420px] min-h-[390px]">
      <div className="h-full bg-gray-300 rounded-lg shadow-md animate-pulse">
        <div className="w-full h-48 rounded-t-lg"></div>
        <div className="p-4">
          <div className="font-bold text-xl mb-2 h-6 bg-gray-200"></div>
          <p className="text-gray-700 text-base h-4 bg-gray-200"></p>
          <div className="mt-4">
            <span className="inline-block text-2xl text-blue-600 mr-2 mb-2 h-6 bg-gray-200"></span>
          </div>
        </div>
      </div>
    </div>
  );
};
