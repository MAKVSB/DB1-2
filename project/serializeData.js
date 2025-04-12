const fs = require('fs');

const animes = new Map();
const animeEpisodes = new Map();
const users = new Map();
const authors = new Map();
const staffAnimeReference = [];
const comments = new Map();
const sources = [];

const randomQualities = ['HD', '4K', 'UHD', 'BR']
const randomLanguages = ['en', 'cs', 'sk', 'jp', 'hu', 'pl', 'ru', 'fr', 'it']
const availableStaffTypes = ['Mangaka', 'Editor', 'Producer', 'Script', 'Writer', 'VoiceAct', 'Music', 'Manager', 'Director' ];

function randomFromArr(arr){
    const random = Math.floor(Math.random() * arr.length);
    return arr[random];
}

function parseStaffType(type){
    if (type.length === 0) {
        return availableStaffTypes[0];
    }
    for (const element of type) {
        if (availableStaffTypes.includes(element)){
            return element;
        }
        // if (['Animator', 'Artist', 'Designer', 'Painter', 'Illustrator', 'Background Artist'].includes(element)){ // 'Mangaka'
        //     return availableStaffTypes[0];
        // }
        if (['Character Designer', 'Composite Artist', 'Photographer', 'CG Artist'].includes(element)){ // 'Editor'
            return availableStaffTypes[1];
        }
        if ([].includes(element)){ // 'Producer'
            return availableStaffTypes[2];
        }
        if (['Storyboard Artist', 'Storyboarder', 'Storyboarder '].includes(element)){ // 'Script'
            return availableStaffTypes[3];
        }
        if (['Scriptwriter', 'Lyricist', 'Translator'].includes(element)){ // 'Writer'
            return availableStaffTypes[4];
        }
        if (['Voice Actor', 'Vocalist', 'Voice Actor'].includes(element)){ // 'VoiceAct'
            return availableStaffTypes[5];
        }
        if (['Musician', 'Band', 'Audio Engineer', 'Guitarist', 'Composer', 'Sound Director', 'Arranger', 'Pianist', 'Choir'].includes(element)){ // 'Music'
            return availableStaffTypes[6];
        }
        if (['Production Manager'].includes(element)){ // 'Manager'
            return availableStaffTypes[7];
        }
        if (['ADR Director', 'Art Director', 'Color Designer', 'Animation Director'].includes(element)){ // 'Director'
            return availableStaffTypes[8];
        }
    };
    return availableStaffTypes[0];
}

function parseStaff(staffData){
    if (authors.get(staffData.id)) return;
    const correctDate = staffData.dateOfBirth.year && staffData.dateOfBirth.month && staffData.dateOfBirth.day;
    authors.set(staffData.id, {
        authorId: staffData.id, 
        firstName: staffData.name.first.replaceAll("'", "''"), 
        lastName: staffData.name.last ? staffData.name.last.replaceAll("'", "''") : staffData.name.last, 
        authorName: staffData.name.userPreferred.replaceAll("'", "''"), 
        birthDate: correctDate ? new Date(staffData.dateOfBirth.year + '/' +  staffData.dateOfBirth.month + '/' + staffData.dateOfBirth.day).toISOString() : 'getdate()', 
        bibliography: staffData.description ? staffData.description.replaceAll("'", "''").slice(0, 490) : staffData.description, 
        photoUrl: staffData.image.medium, 
        type: parseStaffType(staffData.primaryOccupations), //limit to 10 chars 
        created: 'getdate()',
        updated: 'getdate()',
    });
}

function parseSource(sourceData, animeId, animeLang){
    for (const source of sourceData) {
        sources.push({
            animeId, 
            name: source.title.replaceAll("'", "''").slice(0,98), 
            url: source.url, 
            addDate: 'getdate()',
            lastVerifyDate: 'getdate()',
            quality: randomFromArr(randomQualities), 
            created: 'getdate()',
            updated: 'getdate()',
            external: true, 
            paymentRequired: true, 
            language: animeLang, 
            subtitles: 'en, ' + animeLang, 
        }) 
    }
}

const subepisodesMap = new Map();
let subEpisodesid = 1000;
function parseAnime(animeData) {
    animes.set(animeData.id, {
        animeId: animeData.id,
        name: animeData.title.userPreferred,
        length: animeData.duration * animeData.episodes,
        episodeNumber: 0, 
        seriesNumber: 0, 
        desc: animeData.description.substring(0,500), 
        shortDesc: animeData.description.substring(0, 100), 
        created: 'getdate()',
        updated: 'getdate()',
        viewCount: animeData.popularity, 
        language: animeData.countryOfOrigin,
    });
    if (animeData.episodes !== 'NULL' && animeData.episodes > 1){
        for (let episodeNo = 1; episodeNo <= animeData.episodes; episodeNo++) {
            subepisodesMap.set(subEpisodesid, {
                animeId: subEpisodesid,
                name: animeData.title.userPreferred,
                length: animeData.duration,
                episodeNumber: episodeNo, 
                seriesNumber: 0, 
                desc: '', 
                shortDesc: '', 
                created: 'getdate()',
                updated: 'getdate()',
                viewCount: Math.floor(Math.random() * animeData.popularity), 
                language: animeData.countryOfOrigin,
            });
            subEpisodesid++;
        }
        animeEpisodes.set(animeData.id, subepisodesMap);
    }
}

var streetNumber = ['25489', '87459', '35478', '15975', '95125', '78965'];
var streetName = ['A street', 'B street', 'C street', 'D street', 'E street', 'F street'];
var cityName = ['Riyadh', 'Dammam', 'Jedda', 'Tabouk', 'Makka', 'Maddena', 'Haiel'];
var stateName = ['Qassem State', 'North State', 'East State', 'South State', 'West State'];
var zipCode = ['28889', '96459', '35748', '15005', '99625', '71465'];

function getRandomAddress(city) {
    return [
        getRandomElement(streetNumber),
        ' ',
        getRandomElement(streetName),
        ', ',
        city,
        ' ',
        getRandomElement(stateName),
        ', ',
        getRandomElement(zipCode)
    ].join();
}

function getRandomElement(array) {
    if (array instanceof Array) 
        return array[Math.floor(Math.random() * array.length)];
    else 
        return array;
}

function parseUser(userData){
    let city = getRandomElement(cityName);
    users.set(userData.id, {
        userId: userData.id, 
        firstName: userData.name, 
        lastName: (Math.floor(Math.random()) * 3)%2 === 0 ? userData.name : null, 
        email: userData.name+'@google.com', 
        password: '3f3b08eca62c21d76256e6e1d0b8bf99f4efbe376f64335b72f4163a8fc50dba', 
        address: getRandomAddress(city), 
        city, 
        photoUrl: userData.avatar.medium, 
        language: randomFromArr(randomLanguages), 
        role: 0, 
        paid: 0, 
        registeredDate: 'getdate()',
        updated: 'getdate()',
        lastLogin: 'getdate()',
        lastLogin: 'getdate()',
        emailNotifications: (Math.floor(Math.random()) * 3)%2 === 0, 
        emailMarketing: (Math.floor(Math.random()) * 3)%2 === 0,  
    });

}
function parseReviews(reviews, animeId){
    if (reviews?.length){
        for (const review of reviews) {
            parseUser(review.user);
            comments.set(review.id, {
                commentId: review.id, 
                animeId, 
                userId: review.user.id, 
                text: review.body.substring(0, 499), 
                rating: Math.round(review.rating / 10) , 
                created: 'getdate()',
                updated: 'getdate()',
            });
        }
    }
}


for (let index = 10; index < 12; index++) {
    const rawData = JSON.parse(fs.readFileSync('rawData/' + index + '.json'));
    rawData.data.Page.media.forEach(media => {
        if (!media.description || media.type != 'ANIME'){
            return;
        }
        delete media.type;
        delete media.staff.edges;

        media.staff.nodes.forEach(staff => {
            parseStaff(staff);
            if(!staffAnimeReference.find(obj => obj.animeId == media.id && obj.authorId == staff.id)) {
                staffAnimeReference.push({animeId: media.id, authorId: staff.id});
            }
        })
        delete media.staff;


        parseAnime(media);
        delete media.description;

        parseReviews(media.reviews.nodes, media.id);
        parseSource(media.streamingEpisodes, media.id, media.countryOfOrigin)
    });
}

let finalstrings = [];

subepisodesMap.forEach((values,keys)=>{
    finalstrings.push(`INSERT INTO proj_anime
    (animeId, name, length, episodeNumber, seriesNumber, "desc", shortDesc, created, updated, viewCount, language) 
    values 
    (${values.animeId}, '${values.name}', ${values.length}, ${values.episodeNumber}, ${values.seriesNumber}, '${values.desc.replaceAll('\'', '').slice(0, 490)}', '${values.shortDesc.replaceAll('\'', '').slice(0, 98)}', ${values.created}, ${values.updated}, ${values.viewCount}, '${values.language}')

    go
    `);
})

fs.writeFileSync('Skript_Data.sql', finalstrings.join(''));