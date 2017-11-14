using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

#region Additional Namespaces
using Chinook.Data.Entities;
using Chinook.Data.DTOs;
using Chinook.Data.POCOs;
using ChinookSystem.DAL;
using System.ComponentModel;
#endregion

namespace ChinookSystem.BLL
{
    public class PlaylistTracksController
    {
        public List<UserPlaylistTrack> List_TracksForPlaylist(
            string playlistname, string username)
        {
            using (var context = new ChinookContext())
            {
                // What would happen if there is no match for the incoming parameter values?
                // We need to ensure that the results have a valid value
                // This value will be the resolve of a query either a null (not found) or IEnumerable<T> Collection
                // To achieve a valid value encapsulate the query in a .FirstOrDefault               
                var results = (from x in context.Playlists
                               where x.UserName.Equals(username) && x.Name.Equals(playlistname)
                               select x).FirstOrDefault();

                // test if results are null, if not then return null
                if (results != null)
                {
                    // now get the tracks
                    var theTracks = from x in context.PlaylistTracks
                                    where x.PlaylistId.Equals(results.PlaylistId)
                                    orderby x.TrackNumber
                                    select new UserPlaylistTrack
                                    {
                                        TrackID = x.TrackId,
                                        TrackNumber = x.TrackNumber,
                                        TrackName = x.Track.Name,
                                        Milliseconds = x.Track.Milliseconds,
                                        UnitPrice = x.Track.UnitPrice
                                    };
                    return theTracks.ToList();
                }
                else
                {
                    return null;
                }


            }
        }//eom
        public List<UserPlaylistTrack> Add_TrackToPlaylist(string playlistname, string username, int trackid)
        {
            using (var context = new ChinookContext())
            {
                //code to go here
                // Part One: 
                // query to get the playlist id 
                var exists = (from x in context.Playlists
                              where x.UserName.Equals(username) && x.Name.Equals(playlistname)
                              select x).FirstOrDefault();

                // initialize the tracknumber
                int tracknumber = 0;
                // I will need to create an instance of PlaylistTrack
                PlaylistTrack newtrack = null;

                // Determine if a playlist "parent" instances needs to be created
                if (exists == null)
                {
                    // This is a new playlist
                    // create an instance of playlist to add to playlist table
                    exists = new Playlist();
                    exists.Name = playlistname;
                    exists.UserName = username;
                    exists = context.Playlists.Add(exists);
                    // at this time there is no physical pkey
                    // the pseudo pkey is handled by the HashSet
                    tracknumber = 1;
                }
                else
                {
                    // playlist exists
                    // I need to generate the next track number
                    tracknumber = exists.PlaylistTracks.Count() + 1;

                    // validation: in our example a track can ONLY exist once on a particular playlist
                    newtrack = exists.PlaylistTracks.SingleOrDefault(x => x.TrackId == trackid);
                    if (newtrack != null)
                    {
                        throw new Exception("Playlist already has requested track");
                    }
                }
                // Part Two: Add the PlaylistTrack instance
                // use navigation to .Add the new track to PlaylistTrack 
                newtrack = new PlaylistTrack();
                newtrack.TrackId = trackid;
                newtrack.TrackNumber = tracknumber;

                // Note: the pkey for PlaylistID may not yet exist
                // Using navigation one can let HashSet handle the PlaylistId pkey value
                exists.PlaylistTracks.Add(newtrack);

                // physically add all data to the database 
                // commit
                context.SaveChanges();
                return List_TracksForPlaylist(playlistname, username);
            }
        }//eom
        public void MoveTrack(string username, string playlistname, int trackid, int tracknumber, string direction)
        {
            using (var context = new ChinookContext())
            {
                //code to go here 

            }
        }//eom


        public void DeleteTracks(string username, string playlistname, List<int> trackstodelete)
        {
            using (var context = new ChinookContext())
            {
                //code to go here


            }
        }//eom
    }
}
