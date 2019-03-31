using UnityEngine;
using System;
using System.Collections;
using TreeSharpPlus;
using RootMotion.FinalIK;

public class MyBehaviorTree : MonoBehaviour
{
    public Transform[] locations;
    public GameObject[] numberOfParticipants;
    public FullBodyBipedEffector[] eff;
    public InteractionObject[] obj;
    public Transform searchPoint;
    public bool trigger=false;
    public bool inRange = false;
    public float range = 2.0f;

    private BehaviorAgent behaviorAgent;
    private float Timer;
 

	void Start ()
	{
		behaviorAgent = new BehaviorAgent (this.BuildTreeRoot ());
		BehaviorManager.Instance.Register (behaviorAgent);
		behaviorAgent.StartBehavior ();
    }

    // Update is called once per frame
    void Update()
    {
        /*print(Vector3.Distance(numberOfParticipants[0].transform.position, searchPoint.position));
        if (Vector3.Distance(numberOfParticipants[0].transform.position, searchPoint.position) < range)
        {
            print("in range!");
            inRange = true;
        }*/

        Timer += Time.deltaTime;
        //Debug.Log(Timer);
        if (Timer > 20.2f) //&& inRange == true)
            trigger = true;

    }

    public Node ApproachAndOrient(Transform target1, Transform player1pos, int player1Index, Transform target2, Transform player2pos, int player2Index)
    {
        Val<Vector3> p1pos = Val.V(() => player1pos.position);
        Val<Vector3> p2pos = Val.V(() => player2pos.position);
        Val<Vector3> position1 = Val.V(() => target1.position);
        Val<Vector3> position2 = Val.V(() => target2.position);
        return new Sequence(
            new SequenceParallel(
            numberOfParticipants[player1Index].GetComponent<BehaviorMecanim>().Node_GoTo(position1),
            numberOfParticipants[player2Index].GetComponent<BehaviorMecanim>().Node_GoTo(position2)),
            new SequenceParallel(
                numberOfParticipants[player1Index].GetComponent<BehaviorMecanim>().Node_OrientTowards(p2pos),
                numberOfParticipants[player2Index].GetComponent<BehaviorMecanim>().Node_OrientTowards(p1pos)));
    }

    protected Node Converse(int player1Index, int player2Index)
    {
        return
            new DecoratorPrintResult(
                new Sequence(
                numberOfParticipants[player1Index].GetComponent<BehaviorMecanim>().ST_PlayGesture("ACKNOWLEDGE", AnimationLayer.Face, 1000),
                numberOfParticipants[player2Index].GetComponent<BehaviorMecanim>().ST_PlayGesture("HEADSHAKE", AnimationLayer.Face, 1000),
                numberOfParticipants[player1Index].GetComponent<BehaviorMecanim>().ST_PlayGesture("BEINGCOCKY", AnimationLayer.Hand, 1000),
                numberOfParticipants[player2Index].GetComponent<BehaviorMecanim>().ST_PlayGesture("HEADNOD", AnimationLayer.Face, 1000)));
    }

    protected Node Dance(int player1Index, int player2Index)
    {
        return
                new Sequence(
                        numberOfParticipants[player1Index].GetComponent<BehaviorMecanim>().ST_PlayGesture("CROWDPUMP", AnimationLayer.Hand, 1000),
                        new LeafWait(500),
                        numberOfParticipants[player1Index].GetComponent<BehaviorMecanim>().ST_PlayGesture("POINTING", AnimationLayer.Hand, 1000),
                        new LeafWait(500),
                        numberOfParticipants[player2Index].GetComponent<BehaviorMecanim>().ST_PlayGesture("YAWN", AnimationLayer.Face, 1000),
                        new LeafWait(500),
                        numberOfParticipants[player1Index].GetComponent<BehaviorMecanim>().ST_PlayGesture("CUTTHROAT", AnimationLayer.Hand, 1000),
                        new LeafWait(500),
                        numberOfParticipants[player2Index].GetComponent<BehaviorMecanim>().ST_PlayGesture("HEADSHAKE", AnimationLayer.Face, 1000),
                        new LeafWait(500),
                        numberOfParticipants[player1Index].GetComponent<BehaviorMecanim>().ST_PlayGesture("SATNIGHTFEVER", AnimationLayer.Hand, 3000),
                        new LeafWait(500),
                        numberOfParticipants[player2Index].GetComponent<BehaviorMecanim>().ST_PlayGesture("BREAKDANCE", AnimationLayer.Body, 1000),
                        new LeafWait(500),
                        numberOfParticipants[player1Index].GetComponent<BehaviorMecanim>().ST_PlayGesture("SURPRISED", AnimationLayer.Hand, 2000),
                        new LeafWait(500));

    }

    protected Node Gesture(int player1Index, int loop)
    {
        //Val<Vector3> position = Val.V(() => target.position);
        return new DecoratorLoop(loop,
            new Sequence(
                numberOfParticipants[player1Index].GetComponent<BehaviorMecanim>().ST_PlayGesture("CHEER", AnimationLayer.Hand, 1000),
                new LeafWait(1000)));
    }

    protected Node Gesture2(int player1Index, int loop)
    {
        //Val<Vector3> position = Val.V(() => target.position);
        return new DecoratorLoop(loop,
            new Sequence(
                numberOfParticipants[player1Index].GetComponent<BehaviorMecanim>().ST_PlayGesture("CLAP", AnimationLayer.Hand, 1000),
                new LeafWait(1000)));
    }

    protected Node ShakeHands(InteractionObject handShake, Val<FullBodyBipedEffector> effector1, Val<FullBodyBipedEffector> effector2)
    {

        return new SequenceParallel(
            numberOfParticipants[0].GetComponent<BehaviorMecanim>().Node_StartInteraction(effector1, handShake),
            numberOfParticipants[1].GetComponent<BehaviorMecanim>().Node_StartInteraction(effector2, handShake),
            new LeafWait(1000));
    }

    protected Node ApproachAndWait(int player1Index, Transform target)
    {
        Val<Vector3> position = Val.V(() => target.position);
        return new Sequence(
            numberOfParticipants[player1Index].GetComponent<BehaviorMecanim>().Node_GoTo(position),
            new LeafWait(1000));
    }

    public Node Approach(Transform target, int playerIndex)
    {
        Val<Vector3> ppos = Val.V(() => target.position);
        Val <Vector3> pposition = Val.V(() => new Vector3(ppos.Value.x - 0.5f, 0, ppos.Value.z-0));
        return new Sequence(
            numberOfParticipants[playerIndex].GetComponent<BehaviorMecanim>().Node_GoTo(pposition),
            numberOfParticipants[playerIndex].GetComponent<BehaviorMecanim>().Node_OrientTowards(ppos));
    }

    protected Node pressButton(Val<FullBodyBipedEffector> effector, Val<InteractionObject> obj, int player1Index)
    {

        return new Sequence(
            numberOfParticipants[player1Index].GetComponent<BehaviorMecanim>().Node_StartInteraction(effector, obj),
            new LeafWait(1000));
    }

    protected Node BuildTreeRoot()
	{
        int[] playerIndex = new int[numberOfParticipants.Length];
        for (int i = 0; i < playerIndex.Length; i++)
        {
            playerIndex[i] = i;
        }
        return
            new Sequence(
                new SequenceParallel(
                    new Sequence(
                        //this.ST_Approach(searchPoint, playerIndex[0]),
                        //this.ST_pressButton(eff[0], obj[0], playerIndex[0]),
                        this.ApproachAndOrient(locations[0], numberOfParticipants[0].gameObject.transform, playerIndex[0], locations[1], numberOfParticipants[1].gameObject.transform, playerIndex[1]),
                        this.ShakeHands(obj[0], eff[0], eff[0])),
                    new Sequence(
                        this.ApproachAndOrient(locations[2], numberOfParticipants[2].gameObject.transform, playerIndex[2], locations[3], numberOfParticipants[3].gameObject.transform, playerIndex[3]),
                        this.Converse(playerIndex[2], playerIndex[3]))),

                new SequenceParallel(
                    new Sequence(
                        this.ApproachAndOrient(locations[4], numberOfParticipants[0].gameObject.transform, playerIndex[0], locations[7], numberOfParticipants[3].gameObject.transform, playerIndex[3]),
                        new SequenceShuffle(
                            this.Gesture(playerIndex[0], 1),
                            this.Gesture(playerIndex[3], 1)),
                        /*new SelectorShuffle(
                            new Sequence(
                                this.Approach(searchPoint, playerIndex[0]),
                                this.pressButton(eff[0], obj[1], playerIndex[0])),
                            new Sequence(
                                this.Approach(searchPoint, playerIndex[3]),
                                this.pressButton(eff[0], obj[1], playerIndex[3])))),*/

                        new Sequence(
                                this.Approach(searchPoint, playerIndex[0]),
                                this.pressButton(eff[0], obj[1], playerIndex[0]))),

                    new Sequence(
                        this.ApproachAndOrient(locations[5], numberOfParticipants[1].gameObject.transform, playerIndex[1], locations[6], numberOfParticipants[2].gameObject.transform, playerIndex[2]),
                        this.Converse(playerIndex[1], playerIndex[2]))),
                 new SelectorShuffle(
                    new SequenceParallel(
                        new Sequence(
                            this.ApproachAndOrient(locations[8], numberOfParticipants[0].gameObject.transform, playerIndex[0], locations[9], numberOfParticipants[1].gameObject.transform, playerIndex[1]),
                            this.Dance(playerIndex[0], playerIndex[1])),
                        new Sequence(
                            this.Approach(locations[10], playerIndex[2]),
                            this.Gesture2(playerIndex[2], 10)),
                        new Sequence(
                            this.Approach(locations[11], playerIndex[3]),
                            this.Gesture2(playerIndex[3], 10))),
                    new SequenceParallel(
                        new Sequence(
                            this.ApproachAndOrient(locations[8], numberOfParticipants[2].gameObject.transform, playerIndex[2], locations[9], numberOfParticipants[3].gameObject.transform, playerIndex[3]),
                            this.Dance(playerIndex[2], playerIndex[3])),
                        new Sequence(
                            this.Approach(locations[10], playerIndex[0]),
                            this.Gesture2(playerIndex[0], 10)),
                        new Sequence(
                            this.Approach(locations[11], playerIndex[1]),
                            this.Gesture2(playerIndex[1], 10)))));

    }
}
