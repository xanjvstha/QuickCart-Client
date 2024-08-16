import { apiSlice } from "../api/apiSlice";

export const categoryApi = apiSlice.injectEndpoints({
  overrideExisting:true,
  endpoints: (builder) => ({
    addCategory: builder.mutation({
      query: (data) => ({
        url: "http://localhost:7000/api/category/add", 
        method: "POST",
        body: data,
      }),
    }),
    getShowCategory: builder.query({
      query: () => `http://localhost:7000/api/category/show`
    }),
    getProductTypeCategory: builder.query({
      query: (type) => `http://localhost:7000/api/category/show/${type}`
    }),
  }),
});

export const {
 useAddCategoryMutation,
 useGetProductTypeCategoryQuery,
 useGetShowCategoryQuery,
} = categoryApi;
