'use strict';


angular.module('app_work').factory('dataService', function () {
	var countries = ['US', 'Germany', 'UK', 'Japan', 'Italy', 'Greece'],
		products = ['Widget', 'Gadget', 'Doohickey'];

	return {
		getCountries: function () {
			return countries;
		},

		getProducts: function () {
			return products;
		},

		getData: function (count) {
			var data = new wijmo.collections.ObservableArray(),
				i = 0,
				countryId,
				productId;

			for (var i = 0; i < count; i++) {
				countryId = Math.floor(Math.random() * countries.length);
				productId = Math.floor(Math.random() * products.length);
				data.push({
					id: i,
					countryId: countryId,
					productId: productId,
					date: new Date(2014, i % 12, i % 28),
					amount: Math.random() * 10000,
					active: i % 4 === 0
				});
			}
			return data;
		}
	};
});
