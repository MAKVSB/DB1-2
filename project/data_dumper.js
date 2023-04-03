const https = require('https');
const fs = require('fs');

const animes = [];
const users = [];
const authors = [];
const comment = [];


function getMovieData(page){
    return JSON.stringify({
        query: `query lol($page: Int)  {
            Page(page: $page, perPage:50){
                media{
                    id
                    title {
                        userPreferred
                    }
                    episodes
                    duration
                    coverImage{
                        large
                    }
                    startDate{
                        year
                        month
                        day
                    }
                    bannerImage
                    description
                    type
                    averageScore
                    popularity
                    staff {
                        nodes {
                            id
                            name {
                                first
                                middle
                                last
                                full
                                native
                                alternative
                                userPreferred
                            }
                            image {
                                medium
                            }
                            dateOfBirth {
                                year
                                month
                                day
                            }
                            description
                            primaryOccupations
                        }
                        edges {
                            role
                        }
                    }
                    reviews {
                        nodes {
                            id
                            summary
                            body(asHtml: false)
                            rating
                            ratingAmount
                            userRating
                            score
                            private
                            siteUrl
                            createdAt
                            updatedAt
                            user {
                                id
                                name
                                about(asHtml: false)
                                avatar {
                                    medium
                                }
                                isFollowing
                                isFollower
                                isBlocked
                                favourites{anime{nodes{id}}}
                                siteUrl
                                donatorTier
                                donatorBadge
                                moderatorRoles
                                createdAt
                                updatedAt
                                previousNames {
                                    name
                                }
                            }
                        }
                    }
                  	streamingEpisodes {
                        title
                        thumbnail
                        url
                        site
                    }
                    countryOfOrigin
                }
            }
        }`,
        variables: `{
            "page": "${page}"
        }`,
    });
}

function sleep(seconds) 
{
  var e = new Date().getTime() + (seconds * 1000);
  while (new Date().getTime() <= e) {}
}

let currentPage = 0;
setInterval(() => {
    console.log(currentPage);
    const query = getMovieData(currentPage);
    const options = {
      hostname: 'graphql.anilist.co',
      path: '/',
      port: 443,
      method: 'POST',
      headers: {
        'Content-Type': 'application/json',
        'Content-Length': query.length,
        'User-Agent': 'Node',
      },
    };
    
    const req = https.request(options, (res) => {
        let data = '';
        res.on('data', (d) => {
            data += d;
        });
        res.on('end', () => {
            fs.writeFileSync("rawData/" + currentPage + ".json", data)
        });
        res.on('error', () => {
            console.log(currentPage + "ERROR")
        });
    });
    
    req.write(query);
    req.end();
    currentPage++;
}, 2000);