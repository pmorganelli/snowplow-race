using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Player_DestroyTilemaps : MonoBehaviour{

       public Tilemap destructableTilemap;
       private List<Vector3> tileWorldLocations;
       public float rangeDestroy = 2f;
       public bool canExplode = false;
       //public GameObject boomFX;

       public bool isHeadlights = false;

       void Start(){
              TileMapInit();
       }

       void Update(){
              //if ((Input.GetKeyDown("space")) && (canExplode == true)){
              if (canExplode == true){
                     destroyTileArea();
              }
       }

       void TileMapInit(){
              tileWorldLocations = new List<Vector3>();

              foreach (var pos in destructableTilemap.cellBounds.allPositionsWithin){
                     Vector3Int localPlace = new Vector3Int(pos.x, pos.y, pos.z);
                     Vector3 place = destructableTilemap.CellToWorld(localPlace) + new Vector3(0.5f, 0.5f, 0f);

                     if (destructableTilemap.HasTile(localPlace)){
                            tileWorldLocations.Add(place);
                     }
              }
       }

//Truck collisions, to remove snow:
       void OnCollisionEnter2D(Collision2D other){
              if ((!isHeadlights)&&(other.gameObject.tag=="snow")){
                     canExplode = true;
              }
              if ((isHeadlights)&&(other.gameObject.tag=="dark")){
                     canExplode = true;
              }
       }

       void OnCollisionExit2D(Collision2D other){
              if ((!isHeadlights)&&(other.gameObject.tag=="snow")){
                     canExplode = false;
              }
              if ((isHeadlights)&&(other.gameObject.tag=="dark")){
                     canExplode = false;
              }
       }

/*
//light collisions, for darkness:
       void OnTriggerEnter2D(Collider2D other){
              if((isHeadlights)&&(other.gameObject.tag=="dark")){
                     canExplode = true;
              }
              
       }

       void OnTriggerExit2D(Collider2D other){
              if((isHeadlights)&&(other.gameObject.tag=="dark")){
                     canExplode = false;
              }
       }
*/

       void destroyTileArea(){
             foreach (Vector3 tile in tileWorldLocations){
                     if (Vector2.Distance(tile, transform.position) <= rangeDestroy){
                            //Debug.Log("in range");
                            Vector3Int localPlace = destructableTilemap.WorldToCell(tile);
                            if (destructableTilemap.HasTile(localPlace)){
                                   //StartCoroutine(BoomVFX(tile));
                                   destructableTilemap.SetTile(destructableTilemap.WorldToCell(tile), null);
                            }
                     //tileWorldLocations.Remove(tile);
                     }
              }
       }

       //IEnumerator BoomVFX(Vector3 tilePos){
              //GameObject tempVFX = Instantiate(boomFX, tilePos, Quaternion.identity);
              //yield return new WaitForSeconds(.5f);
              //Destroy(tempVFX);
       //}

       //NOTE: To help see the attack sphere in editor:
       void OnDrawGizmosSelected(){
              Gizmos.DrawWireSphere(transform.position, rangeDestroy);
       }
}