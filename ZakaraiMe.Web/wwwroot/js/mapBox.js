mapboxgl.accessToken = 'pk.eyJ1IjoiZHVuZHVybHVua2EiLCJhIjoiY2pzcWd6OGR6MTQ3NTQ5cjJmMDd5eTJmbyJ9.TXbTIkJ-3Sp4t7kvj94idA';
var mapboxDirections = new MapboxDirections({
    accessToken: mapboxgl.accessToken
});

function initMap(lat, lng) {
    let zoom = 9;

    if (lat == 0 || lng == 0) {
        lat = 25.5; 
        lng = 42.75;
        zoom = 6;
    }
    
    var map = new mapboxgl.Map({
        container: 'map',
        style: 'mapbox://styles/mapbox/streets-v9',
        center: [lat, lng],
        zoom: zoom
    });
    
    map.addControl(mapboxDirections, 'top-left');

    var obj = mapboxDirections.getDestination();

    console.log(Object.getOwnPropertyNames(obj));
}


